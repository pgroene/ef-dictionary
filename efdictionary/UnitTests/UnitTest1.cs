using efdictionary;
using efdictionary.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GivenData_WhenPuttingChildObjectsInDictionary_ThenFKReferencesAreChanged()
        {
            var options = DbContextHelper.CreateDbContextOptions<TestDbContext>();

            using (var sut = new TestDbContext(options))
            {
                //A
                Add(sut, "C1", "CC2", 4);
                Add(sut, "C2", "CC2", 4);
                Add(sut, "C3", "CC2", 4);
                Add(sut, "C4", "CC2", 4);

                Add(sut, "C1", "CC1", 4);
                Add(sut, "C2", "CC1", 4);
                Add(sut, "C3", "CC1", 4);
                Add(sut, "C4", "CC1", 4);

                sut.SaveChanges();
                Assert.IsFalse(sut.ChangeTracker.HasChanges());


                var dict = new Dictionary<string, List<Child>>();
                foreach (var code in new[] { "C1", "C2" })
                {
                    var items = (sut.Parents
                               .Include(r => r.Childs)
                               .Where(r => r.Code1 == code)
                               .ToList())
                               .ToDictionary<Parent, string>(r => r.Code2);

                    foreach (var key in items.Keys)
                    {
                        if (dict.ContainsKey(key))
                            dict[key].AddRange(items[key].Childs); //  <<<<<-------------- Triggers unexpected behavoir
                        else
                        {
                            dict.Add(key, items[key].Childs);
                        }
                    }
                }

                //Assert.IsFalse(sut.ChangeTracker.HasChanges());// Fails
                sut.SaveChanges();

                //Expected nothing changed

                Assert.AreEqual(4, sut.Parents.Where(p => p.Code1 == "C1").First().Childs.Count());//fails 8
                Assert.AreEqual(4, sut.Parents.Where(p => p.Code1 == "C2").First().Childs.Count());//fails 8
                Assert.AreEqual(4, sut.Parents.Where(p => p.Code1 == "C1").Last().Childs.Count());//fails 0
                Assert.AreEqual(4, sut.Parents.Where(p => p.Code1 == "C2").Last().Childs.Count());//fails 0
            }
        }

        private void Add(TestDbContext context, string code1, string code2, int clildCount)
        {
            var parent = new Parent()
            {
                Code1 = code1,
                Code2 = code2,
            };
            for (int i = 0; i < clildCount; i++)
            {
                parent.Childs.Add(new Child() { });
            }

            context.Add(parent);
        }
    }
}
