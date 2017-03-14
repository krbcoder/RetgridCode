using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sabio.Web.Services;
using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Responses;
using Sabio.Web.Exceptions;
using System.Threading.Tasks;
using Sabio.Web.Domain;
using Sabio.Web.Models;
using System.Collections;
using Newtonsoft.Json;

namespace Sabio.Web.Controllers.Api
{
    [RoutePrefix("api/openhouses")]

    public class OpenHousesApiController : BaseApiController
    {
        IOpenHouseService _openHouseService = null;
        public OpenHousesApiController(IOpenHouseService openHouseService)
        {
            _openHouseService = openHouseService;
        }


        [Route("{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetOpenHouseById(int id)
        {

            HttpStatusCode statusCode = HttpStatusCode.OK;
            BaseResponse response = null;

            try
            {

                response = new ItemResponse<OpenHouseAgentDetail>();
                ((ItemResponse<OpenHouseAgentDetail>)response).Item = _openHouseService.GetOpenHouseWithAgentDetail(id);
                statusCode = HttpStatusCode.OK;
                
            }

            catch (Exception ex)
            {
                // general error
                statusCode = HttpStatusCode.ExpectationFailed;
                response = new ErrorResponse(ex.Message);

            }

            return Request.CreateResponse(statusCode, response);
        }

        [Route]
        [HttpPost]
        public HttpResponseMessage GetActiveOpenHousesBySearchCriteria(OpenHouseDatesSearchRequest model)
        {

            HttpStatusCode statusCode = HttpStatusCode.OK;
            BaseResponse response = null;

            // check initial request model
            if (!ModelState.IsValid)
            {
                return ErrorList(ModelState.Values);
            }

            try
            {
                response = new ItemsResponse<OpenHouseBase>();
                ((ItemsResponse<OpenHouseBase>)response).Items = _openHouseService.GetActiveOpenHousesBySearch(model);
                statusCode = HttpStatusCode.OK;
            }

            catch (Exception ex)
            {
                // general error
                statusCode = HttpStatusCode.ExpectationFailed;
                response = new ErrorResponse(ex.Message);
            }

            return Request.CreateResponse(statusCode, response);
        }

        //create visitors
        [Route("{id:int}/visitors/create")]
        [HttpPost]
        public HttpResponseMessage PostVisitor(VisitorAddRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            ItemResponse<int> response = new ItemResponse<int>();
            response.Item = _openHouseService.InsertVisitor(model);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        //update visitors
        [Route("visitors/{id:int}/edit")]
        [HttpPut]
        public HttpResponseMessage UpdateVisitor(VisitorUpdateRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            _openHouseService.UpdateVisitor(model);
            SuccessResponse response = new SuccessResponse();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        //get visitors by open house id
        [Route("{id:int}/visitors")]
        [HttpGet]
        public HttpResponseMessage GetVisitorsByOpenHouseId(int Id)
        {
            ItemsResponse<Visitor> response = new ItemsResponse<Visitor>();
            response.Items = _openHouseService.SelectVisitorsByOpenHouseId(Id);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        //send agent invite
        [Route("visitors/invite")]
        [HttpPost]
        public HttpResponseMessage Invite(Hashtable model)
        {
            if(!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            var v = model["visitor"].ToString();
            var a = model["agent"].ToString();

            OpenHouseVisitor visitor = JsonConvert.DeserializeObject<OpenHouseVisitor>(v);
            Person agent = JsonConvert.DeserializeObject<Person>(a);

            AgentInviteRequest requestModel = new AgentInviteRequest(visitor, agent);
            ItemResponse<Task<bool>> response = new ItemResponse<Task<bool>>();
            response.Item = MessagingService.Invite(requestModel);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        //send property update
        [Route("visitors/propertyUpdate")]
        [HttpPost]
        public HttpResponseMessage UpdateEmail(Hashtable model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            var v = model["visitor"].ToString();
            var a = model["agent"].ToString();
            var e = model["email"].ToString();

            Visitor visitor = JsonConvert.DeserializeObject<Visitor>(v);
            OpenHouseAgentDetail agent = JsonConvert.DeserializeObject<OpenHouseAgentDetail>(a);
            Email email = JsonConvert.DeserializeObject<Email>(e);

            EmailPropertyUpdateRequest requestModel = new EmailPropertyUpdateRequest(visitor, agent, email);
            ItemResponse<Task<bool>> response = new ItemResponse<Task<bool>>();
            response.Item = MessagingService.UpdateEmail(requestModel);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        //unsubscribe visitor
        [Route("visitors/{id:int}/unsubscribe")]
        [HttpPut]
        public HttpResponseMessage Unsubscribe(int Id)
        {
            SuccessResponse response = new SuccessResponse();
            _openHouseService.Unsubscribe(Id);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

    }
}
