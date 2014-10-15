using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BouvetCodeCamp.Domene;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.Domene.InputModels;
using BouvetCodeCamp.DomeneTjenester.Interfaces;
using Moq;
using NUnit.Framework;

namespace BouvetCodeCamp.UnitTest
{
    using GameApi = DomeneTjenester.GameApi;

    [TestFixture]
    public class GameApiTest
    {
        private IGameApi _gameApi;
        private readonly Mock<IPostGameService> _postGameService = new Mock<IPostGameService>();
        private readonly Mock<ILagGameService> _lagGameService = new Mock<ILagGameService>();
        private readonly Mock<IService<Lag>> _lagService = new Mock<IService<Lag>>();
        private readonly Mock<IService<GameState>> _gameStateService = new Mock<IService<GameState>>();
        private readonly Mock<IKoordinatVerifier> _koordinatVerifier = new Mock<IKoordinatVerifier>();
        private readonly Mock<IPoengService> _poengServiceMock = new Mock<IPoengService>();

        [SetUp]
        public void Setup()
        {
            _gameApi = new GameApi(
                _postGameService.Object,
                _lagGameService.Object,
                _lagService.Object,
                _koordinatVerifier.Object,
                _gameStateService.Object,
                _poengServiceMock.Object
                );
        }

        [Test]
        [ExpectedException(typeof(MeldingException))]
        public async void SendMelding_Lengde_IkkeInt_Exception()
        {
            var melding = new MeldingInputModell
            {
                Type = MeldingType.Lengde,
                Innhold = "juks"
            };
            await _gameApi.SendMelding(melding);
        }

        [Test]
        [ExpectedException(typeof(MeldingException))]
        public async void SendMelding_Fritekst_TekstOver256_Exception()
        {
            var melding = new MeldingInputModell
            {
                Type = MeldingType.Fritekst,
                Innhold = "UTiBm8m3wOvmBLrwO26NRPHI2o7pwPUSuxPcjn1A6ybVZ88OBwsWO9Z0FAfLtowUjwJGJLIg4BNXS8GozxBqZjKgE3WUcfNhbBUxgJVEty4LJwpCvTjkSux1njsA6pG0TDmrE04v62kHmwE0zip2gP5XG0Ew43G3hg4KhjYWDtQ5bTmKB15qkXqU0gYTZUTR10ZGIzoeaYuvVDOoc1CHvwSApqkMUoNRycnv1QzH62pk7SPY8n5HxlfMnsF8eJSfI"
            };
            Assert.IsTrue(melding.Innhold.Length > 256);
            await _gameApi.SendMelding(melding);
        }

        [Test]
        public async void SendMelding_Fritekst_ErUnder256_Gyldig()
        {
            var melding = new MeldingInputModell
            {
                Type = MeldingType.Fritekst,
                Innhold = "UTiBm8m3wOvmBLrwO26NRPHI2o7pwPUSuxPcjn1A6ybVZ88OBwsWO9Z0FAfLtowUjwJGJLIg4BNXS8GozxBqZjKgE3WUcfNhbBUxgJVEty4LJwpCvTjkSux1njsA6pG0TDmrE04v62kHmwE0zip2gP5XG0Ew43G3hg4KhjYWDtQ5bTmKB15qkXqU0gYTZUTR10ZGIzeaYuvVDOoc1CHvwSApqkMUoNRycnv1QzH62pk7SPY8n5HxlfMnsF8eJSfI"
            };
            _lagGameService.Setup(x => x.HentLagMedLagId(It.IsAny<string>())).Returns(new Lag
            {
                Meldinger = new List<Melding>()
            });

            await _gameApi.SendMelding(melding);

            Assert.IsTrue(melding.Innhold.Length <= 256);
            Assert.IsTrue(true);
        }


        [Test]
        [ExpectedException(typeof(MeldingException))]
        public async void SendMelding_Stopp_NotBool_Exception()
        {
            var melding = new MeldingInputModell
            {
                Type = MeldingType.Stopp,
                Innhold = "juks"
            };

            await _gameApi.SendMelding(melding);
        }

        [Test]
        public async void SendMelding_Stopp_Bool_Success()
        {
            var melding = new MeldingInputModell
            {
                Type = MeldingType.Stopp,
                Innhold = "false"
            };
            _lagGameService.Setup(x => x.HentLagMedLagId(It.IsAny<string>())).Returns(new Lag
            {
                Meldinger = new List<Melding>()
            });

            await _gameApi.SendMelding(melding);
            Assert.IsTrue(true);
        }

        [Test]
        [ExpectedException(typeof(MeldingException))]
        public async void SendMelding_Himmelretning_Invalid_Exception()
        {
            var melding = new MeldingInputModell
            {
                Type = MeldingType.Himmelretning,
                Innhold = "ikke en himmelretning"
            };

            await _gameApi.SendMelding(melding);
        }

        [Test]
        public async void SendMelding_Himmelretning_North_Success()
        {
            var melding = new MeldingInputModell
            {
                Type = MeldingType.Himmelretning,
                Innhold = "North"
            };
            _lagGameService.Setup(x => x.HentLagMedLagId(It.IsAny<string>())).Returns(new Lag
            {
                Meldinger = new List<Melding>()
            });

            await _gameApi.SendMelding(melding);
            Assert.IsTrue(true);
        }

        [Test]
        public async void SendMelding_Himmelretning_East_Success()
        {
            var melding = new MeldingInputModell
            {
                Type = MeldingType.Himmelretning,
                Innhold = "East"
            };
            _lagGameService.Setup(x => x.HentLagMedLagId(It.IsAny<string>())).Returns(new Lag
            {
                Meldinger = new List<Melding>()
            });

            await _gameApi.SendMelding(melding);
            Assert.IsTrue(true);
        }

        [Test]
        public async void SendMelding_Himmelretning_West_Success()
        {
            var melding = new MeldingInputModell
            {
                Type = MeldingType.Himmelretning,
                Innhold = "West"
            };
            _lagGameService.Setup(x => x.HentLagMedLagId(It.IsAny<string>())).Returns(new Lag
            {
                Meldinger = new List<Melding>()
            });

            await _gameApi.SendMelding(melding);
            Assert.IsTrue(true);
        }

        [Test]
        public async void SendMelding_Himmelretning_South_Success()
        {
            var melding = new MeldingInputModell
            {
                Type = MeldingType.Himmelretning,
                Innhold = "South"
            };
            _lagGameService.Setup(x => x.HentLagMedLagId(It.IsAny<string>())).Returns(new Lag
            {
                Meldinger = new List<Melding>()
            });

            await _gameApi.SendMelding(melding);
            Assert.IsTrue(true);
        }

        [Test]
        [ExpectedException(typeof(MeldingException))]
        public async void SendMelding_Himmelretning_Integer_Exception()
        {
            var melding = new MeldingInputModell
            {
                Type = MeldingType.Himmelretning,
                Innhold = "1"
            };
            _lagGameService.Setup(x => x.HentLagMedLagId(It.IsAny<string>())).Returns(new Lag
            {
                Meldinger = new List<Melding>()
            });

            await _gameApi.SendMelding(melding);
            Assert.IsTrue(true);
        }
        [Test]
        [ExpectedException]
        public async void SendMelding_Himmelretning_Null_Exception()
        {
            var melding = new MeldingInputModell
            {
                Type = MeldingType.Himmelretning,
                Innhold = null
            };
            _lagGameService.Setup(x => x.HentLagMedLagId(It.IsAny<string>())).Returns(new Lag
            {
                Meldinger = new List<Melding>()
            });

            await _gameApi.SendMelding(melding);
            Assert.IsTrue(true);
        }


        [Test]
        public void ErLagPifInnenInfeksjonssone_ErInnenInfeksjonssone_ReturnsTrue()
        {
            var gamestate = new GameState { InfisertPolygon = new InfisertPolygon()};
            var pif = new PifPosisjon();

            _lagGameService.Setup(x => x.HentSistePifPosisjon(It.IsAny<string>())).Returns(() => pif);
            _gameStateService.Setup(x => x.Hent(string.Empty)).Returns(() => gamestate);
            _koordinatVerifier.Setup(
                x => x.KoordinatErInnenforPolygonet(pif.Posisjon, gamestate.InfisertPolygon.Koordinater)).Returns(true);

            var result = _gameApi.ErLagPifInnenInfeksjonssone(String.Empty);

            Assert.IsTrue(result);
        }
    }
}
