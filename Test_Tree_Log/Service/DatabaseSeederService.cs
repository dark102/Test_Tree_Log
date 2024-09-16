using Microsoft.EntityFrameworkCore;
using Test_Tree_Log.Models;

namespace Test_Tree_Log.Service
{
    public class DatabaseSeederService : IDatabaseSeederService
    {
        private ApplicationDbContext dbContext;
        private ILogger<DatabaseSeederService> logger;
        public DatabaseSeederService(ApplicationDbContext dbContext, ILogger<DatabaseSeederService> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }
        public async Task SeedAsync()
        {
            await dbContext.Database.MigrateAsync();
            await SeedDefaultUsersAsync();
        }

        /************ DEFAULT USERS **************/

        private async Task SeedDefaultUsersAsync()
        {
            if (await dbContext.tree.CountAsync() == 0)
            {
                try
                {
                    Tree tree = new Tree
                    {
                        name = "root",
                        children = new List<Child>()
                    };
                    await dbContext.tree.AddAsync(tree);
                    await dbContext.SaveChangesAsync();
                    var childrenList = new List<Child>()
                    {
                        new Child
                            {
                                name = "children1",
                                children = new List<Child>()
                                {
                                    new Child
                                    {
                                        name = "children3",
                                        children = new List<Child>(),
                                        treeid = tree.id
                                    },
                                    new Child
                                    {
                                        name = "children4",
                                        children = new List<Child>(){
                                            new Child
                                            {
                                                name = "children5",
                                                children = new List<Child>(),
                                                treeid = tree.id
                                            }
                                        },
                                        treeid = tree.id
                                    }
                                },
                                treeid = tree.id
                            },
                        new Child
                        {
                            name = "children2",
                            children = new List<Child>(),
                            treeid = tree.id
                        }
                    };
                    dbContext.child.AddRange(childrenList);
                    await dbContext.SaveChangesAsync();

                    logger.LogInformation("The root data is generated");
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error when generating root data {ex.Message}");

                }
            }
        }
    }
}
