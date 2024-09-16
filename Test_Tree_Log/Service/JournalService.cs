
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Diagnostics;
using Test_Tree_Log.Models;

namespace Test_Tree_Log.Service
{
    public class JournalService : IJournalService
    {
        private readonly ApplicationDbContext _context;
        public JournalService(IConfiguration configuration)
        {
            DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            optionsBuilder.UseLazyLoadingProxies();
            _context = new ApplicationDbContext(optionsBuilder.Options);
        }
        public async Task<ExceptionJournal> SetNewJournaLine(Exception exception, HttpContext httpContext)
        {
            try
            {
                string? bodyRequest = null;
                string? eventID = httpContext.TraceIdentifier;
                string? pathRequest = httpContext.Request.Path;
                ExceptionData? exceptionData = await getExceptionData(exception);
                string? paramRequest = httpContext.Request.QueryString.Value
    ;
                if (httpContext.Request.Method == "POST")
                {
                    using (StreamReader sr = new StreamReader(httpContext.Request.Body))
                    {
                        bodyRequest = String.IsNullOrWhiteSpace(await sr.ReadToEndAsync()) ? null : await sr.ReadToEndAsync();
                    }
                }
                ExceptionJournal exceptionJournal = new ExceptionJournal
                {
                    bodyRequest = bodyRequest,
                    eventID = eventID,
                    pathRequest = pathRequest,
                    exceptionData = exceptionData,
                    paramRequest = paramRequest
                };
                _context.exceptionJournal.Add(exceptionJournal);
                await _context.SaveChangesAsync();
                return exceptionJournal;

            } 
            catch (Exception ex) 
            {
                return null;
            
            }
            
            
        }
        public async Task<(List<ExceptionJournal> list, int allCount)> GetJournaList(int skip, int take, JournalFilter filter)
        {
            var list = _context.exceptionJournal;
            if (filter.search != null)
                list.Where(x => x.exceptionData.exceptionMessage.Contains(filter.search));
            if(filter.from != null)
            {
                list.Where(x => x.exceptionData.createdDate.Ticks>filter.from.Value.Ticks);
            }
            if (filter.to != null)
            {
                list.Where(x => x.exceptionData.createdDate.Ticks < filter.to.Value.Ticks);
            }
            return (await list.Skip(skip).Take(take).ToListAsync(), await _context.exceptionJournal.CountAsync());
        }
        public async Task<ExceptionJournal> GetJournaById(int Id)
        {
            return await _context.exceptionJournal.FirstOrDefaultAsync(x => x.id == Id);
        }
        private async Task<ExceptionData> getExceptionData(Exception ex)
        {
            ExceptionData exceptionData = new ExceptionData()
            {
                exceptionType = ex.GetType().FullName,
                stackTracert = ex.StackTrace,
                inerExceptionData = ex.InnerException == null ? null : await getExceptionData(ex.InnerException),
                exceptionMessage = ex.Message
            };
            return exceptionData;
        }

        
    }
}
