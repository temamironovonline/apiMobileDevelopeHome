using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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


        [Route("api/Warehouses/sortByCountOrPrice")]
        [HttpGet] // There are HttpGet, HttpPost, HttpPut, HttpDelete.
        public async Task<IHttpActionResult> SortByCountOrPrice(int typeOfSort, string nameProduct)
        {
            Regex checkName = new Regex($@"{nameProduct}.*");
            switch (typeOfSort)
            {
                case 0:
                    return Ok(db.Warehouse.ToList().ConvertAll(x => new WarehouseProduct(x)).Where(x => checkName.IsMatch(x.productName)));
                case 1:
                    return Ok(db.Warehouse.ToList().ConvertAll(x => new WarehouseProduct(x)).Where(x => checkName.IsMatch(x.productName)).OrderBy(x => x.productPrice));
                case 2:
                    return Ok(db.Warehouse.ToList().ConvertAll(x => new WarehouseProduct(x)).Where(x => checkName.IsMatch(x.productName)).OrderByDescending(x => x.productPrice));
                case 3:
                    return Ok(db.Warehouse.ToList().ConvertAll(x => new WarehouseProduct(x)).Where(x => checkName.IsMatch(x.productName)).OrderBy(x => x.productCount));
                case 4:
                    return Ok(db.Warehouse.ToList().ConvertAll(x => new WarehouseProduct(x)).Where(x => checkName.IsMatch(x.productName)).OrderByDescending(x => x.productCount));
                default: return BadRequest();
            }
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