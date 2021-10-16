using CalorieTracker.Data;
using CalorieTracker.Handlers.Users;
using CalorieTracker.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CalorieTracker.Test.Handlers.Users {
    public class GetUsersHandlerShould {
        [Fact]
        public async Task ReturnAllUsers() {
            var options = new DbContextOptionsBuilder<CalorieTrackerContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var users = Enumerable.Range(0, 10).Select(x =>
                             new User {
                                 Email = $"{x}@email.com",
                                 Name = $"{x} name"
                             });

            using (var context = new CalorieTrackerContext(options)) {

                await context.Users.AddRangeAsync(users);

                await context.SaveChangesAsync();
            }

            using (var context = new CalorieTrackerContext(options)) {

                var getUsersHandler = new GetUsersHandler(context);
                var getUsersRequest = new GetUsersRequest();

                var result = await getUsersHandler.Handle(getUsersRequest, CancellationToken.None);

                Assert.NotNull(result);
                Assert.Equal(users.Count(), result.Count());

                foreach(var userItem in users) {
                   var dbUser = result.FirstOrDefault(r => r.Email == userItem.Email);
                    
                    Assert.NotNull(dbUser);
                    Assert.Equal(userItem.Name, dbUser.Name);
                }
            }
        }
    }
}
