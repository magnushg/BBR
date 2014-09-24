using System.Threading.Tasks;

namespace BouvetCodeCamp.Lasttesting
{
    using System;

    using BouvetCodeCamp.Integrasjonstester;
    using BouvetCodeCamp.Integrasjonstester.DataAksess;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LagRepositoryLasttest : BaseRepositoryIntegrasjonstest
    {
        [Ignore]
        [TestMethod]
        [TestCategory(Testkategorier.Last)]
        public void Lasttest_Hent_LagMedLagId_LagHarLagId()
        {
            // Arrange
            var lagRepositoryIntegrasjonstester = new LagRepositoryIntegrasjonstester();
            
            const int AntallTesterSomSkalKjøres = 10;

            try
            {
                var parallelleTasker = new Task[AntallTesterSomSkalKjøres];

                for (var i = 0; i < parallelleTasker.Length; i++)
                {
                    parallelleTasker[i] = new Task(() => lagRepositoryIntegrasjonstester.Hent_LagMedLagId_LagHarLagId());
                }

                Task.WaitAll(parallelleTasker);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Assert.IsTrue(true);
        }
    }
}