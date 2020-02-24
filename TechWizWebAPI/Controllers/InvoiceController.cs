using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections;
using TechWizWebAPI.Models;

namespace TechWizWebAPI.Controllers
{
    public class InvoiceController : ApiController
    {
        // GET: api/Invoice
        public ArrayList Get()
        {
            InvoicePersistence ip = new InvoicePersistence();
            return ip.getInvoices();
        }

        // GET: api/Invoice/5
        public Invoice Get(long id)
        {
            InvoicePersistence ip = new InvoicePersistence();
            Invoice invoice = ip.getInvoice(id);
            return invoice;
        }

        // POST: api/Invoice
        public HttpResponseMessage  Post([FromBody]Invoice value)
        {
            InvoicePersistence ip = new InvoicePersistence();
            long id;
            id = ip.saveInvoice(value);
            value.invoiceID = id;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Location = new Uri(Request.RequestUri, String.Format("invoice/{0}", id));
            return response;
        }

        // PUT: api/Invoice/5
        public HttpResponseMessage Put(long id, [FromBody]Invoice value)
        {
            InvoicePersistence ip = new InvoicePersistence();
            bool recordExisted = false;
            recordExisted = ip.updateInvoice(id, value);

            HttpResponseMessage response;
            if (recordExisted)
            {
                response = Request.CreateResponse(HttpStatusCode.NoContent);
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return response;
        }

        // DELETE: api/Invoice/5
        public HttpResponseMessage Delete(long id)
        {
            InvoicePersistence ip = new InvoicePersistence();
            bool recordExisted = false;
            recordExisted = ip.deleteInvoice(id);

            HttpResponseMessage response;
            if (recordExisted)
            {
                response = Request.CreateResponse(HttpStatusCode.NoContent);
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return response;
        }
    }
}
