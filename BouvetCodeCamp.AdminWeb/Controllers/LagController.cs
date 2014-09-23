using System.Web.Mvc;

namespace BouvetCodeCamp.AdminWeb.Controllers
{
    using System.Net;
    using System.Threading.Tasks;

    using BouvetCodeCamp.Dataaksess.Interfaces;
    using BouvetCodeCamp.Felles.Entiteter;

    public class LagController : Controller
    {
        private readonly IRepository<Lag> lagRepository;

        public LagController(IRepository<Lag> lagRepository)
        {
            this.lagRepository = lagRepository;
        }

        public async Task<ActionResult> Index()
        {
            var items = await lagRepository.HentAlle();
            return View(items);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Lag lag)
        {
            if (ModelState.IsValid)
            {
                await lagRepository.Opprett(lag);
                return RedirectToAction("Index");
            }
            return View(lag);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Lag lag)
        {
            if (ModelState.IsValid)
            {
                await lagRepository.Oppdater(lag);
                return RedirectToAction("Index");
            }
            return View(lag);
        }

        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var lag = await lagRepository.Hent(id);
            if (lag == null)
            {
                return HttpNotFound();
            }
            return View(lag);
        }

        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var lag = await lagRepository.Hent(id);
            if (lag == null)
            {
                return HttpNotFound();
            }
            return View(lag);
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var lag = await lagRepository.Hent(id);
            if (lag == null)
            {
                return HttpNotFound();
            }

            await lagRepository.Slett(lag);
            return RedirectToAction("Index");
        }
    }
}