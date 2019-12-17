using Engine.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestEngine.ViewModels
{        
    [TestClass]
    public class TestGameSession
    {
        [TestMethod]
        public void TestCreateGameSession()
        {
            var gameSession = new GameSession();

            Assert.IsNotNull(gameSession.CurrentPlayer);
            Assert.AreEqual("Town Square", gameSession.CurrentLocation.Name);
        }

        [TestMethod]
        public void TestPlayerMovesHomeAndIsCompletelyHealedWhenKilled()
        {
            var gs = new GameSession();

            gs.CurrentPlayer.TakeDamage(9999);

            Assert.AreEqual("Home",gs.CurrentLocation.Name);
            Assert.AreEqual(gs.CurrentPlayer.MaximumHitPoints, gs.CurrentPlayer.CurrentHitPoints);
        }
    }
}