using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using apiMobileDevelope.Models;

namespace apiMobileDevelope.Controllers
{
    public class WarehousesController : ApiController
    {
        private Warehouse_SergeichevEntities db = new Warehouse_SergeichevEntities();

        // GET: api/Warehouses
        [ResponseType(typeof(List<WarehouseProduct>))]
        public IHttpActionResult GetWarehouse()
        {
            return Ok(db.Warehouse.ToList().ConvertAll(x => new WarehouseProduct(x)));
        }

        // GET: api/Warehouses/5
        [ResponseType(typeof(Warehouse))]
        public IHttpActionResult GetWarehouse(int id)
        {
            Warehouse warehouse = db.Warehouse.Find(id);
            if (warehouse == null)
            {
                return NotFound();
            }

            return Ok(warehouse);
        }

        // PUT: api/Warehouses/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutWarehouse(int id, Warehouse warehouse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != warehouse.Code_Product)
            {
                return BadRequest();
            }

            db.Entry(warehouse).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WarehouseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Warehouses
        [ResponseType(typeof(Warehouse))]
        public IHttpActionResult PostWarehouse(Warehouse warehouse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Warehouse.Add(warehouse);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = warehouse.Code_Product }, warehouse);
        }

        // DELETE: api/Warehouses/5
        [ResponseType(typeof(Warehouse))]
        public IHttpActionResult DeleteWarehouse(int id)
        {
            Warehouse warehouse = db.Warehouse.Find(id);
            if (warehouse == null)
            {
                return NotFound();
            }

            db.Warehouse.Remove(warehouse);
            db.SaveChanges();

            return Ok(warehouse);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WarehouseExists(int id)
        {
            return db.Warehouse.Count(e => e.Code_Product == id) > 0;
        }
    }
}