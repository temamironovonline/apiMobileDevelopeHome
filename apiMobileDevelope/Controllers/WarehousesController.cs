﻿using System.Collections.Generic;
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
        // https://ssfb.ngknn.local/NGKNN/%D1%81%D0%B5%D1%80%D0%B3%D0%B5%D0%B8%D1%87%D0%B5%D0%B2%D0%B0%D0%B4/Help

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

        [ResponseType(typeof(WarehouseProduct))]
        public IHttpActionResult GetWarehouse(string nameProduct)
        {
            Regex checkName = new Regex($@"^{Regex.Escape(nameProduct)}.*");
            return Ok(db.Warehouse.ToList().ConvertAll(x => new WarehouseProduct(x)).Where(x => checkName.IsMatch(x.productName)));
        }

        [Route("api/Warehouses/sortByCount")]
        [HttpGet] // There are HttpGet, HttpPost, HttpPut, HttpDelete.
        public async Task<IHttpActionResult> SortByCount(bool a)
        {
            if (a)
                return Ok(db.Warehouse.ToList().ConvertAll(x => new WarehouseProduct(x)).OrderBy(x => x.productCount));
            else
                return Ok(db.Warehouse.ToList().ConvertAll(x => new WarehouseProduct(x)).OrderByDescending(x => x.productCount));
        }

        [Route("api/Warehouses/sortByPrice")]
        [HttpGet] // There are HttpGet, HttpPost, HttpPut, HttpDelete.
        public async Task<IHttpActionResult> SortByPrice(bool a)
        {
            if (a)
                return Ok(db.Warehouse.ToList().ConvertAll(x => new WarehouseProduct(x)).OrderBy(x => x.productPrice));
            else
                return Ok(db.Warehouse.ToList().ConvertAll(x => new WarehouseProduct(x)).OrderByDescending(x => x.productCount));
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