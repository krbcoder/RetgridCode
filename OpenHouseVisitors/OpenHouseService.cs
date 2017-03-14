using Sabio.Data;
using Sabio.Web.Domain;
using Sabio.Web.Models;
using Sabio.Web.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sabio.Web.Services
{
    public class OpenHouseService : BaseService, IOpenHouseService
    {

        public OpenHouseBase Get(int id)
        {
            OpenHouseBase openHouse = new OpenHouseBase();

            DataProvider.ExecuteCmd(GetConnection, "dbo.OpenHouses_SelectById"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Id", id);

               }, map: delegate (IDataReader reader, short set)
               {

                   openHouse = MapOpenHouseReader(reader);
               }
               );

            return openHouse;
        }

        public List<OpenHouseBase> GetActiveOpenHousesBySearch(OpenHouseDatesSearchRequest model)
        {
            List<OpenHouseBase> openHouseList = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.OpenHouses_SelectAllActiveBySearch"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@StartDate", model.StartDate);
                   if (model.EndDate.HasValue && (model.EndDate > model.StartDate))
                   {
                       paramCollection.AddWithValue("@EndDate", model.EndDate);
                   }
                   else
                   {
                       paramCollection.AddWithValue("@EndDate", null);
                   }
                   paramCollection.AddWithValue("@ListingKeyNumeric", model.ListingKeyNumeric);
                   paramCollection.AddWithValue("@MlsListingId", model.MlsListingId);
                   paramCollection.AddWithValue("@PostalCode", model.PostalCode);
                   paramCollection.AddWithValue("@CityName", model.CityName);
                   paramCollection.AddWithValue("@MaxRows", model.MaxRows);

               }, map: delegate (IDataReader reader, short set)
               {
                   OpenHouseBase openHouse = MapOpenHouseReader(reader);

                   if (openHouseList == null)
                   {
                       openHouseList = new List<OpenHouseBase>();
                   }

                   openHouseList.Add(openHouse);
               }
               );

            return openHouseList;
        }

        public OpenHouseAgentDetail GetOpenHouseWithAgentDetail(int id)
        {
            OpenHouseAgentDetail openHouse = new OpenHouseAgentDetail();

            DataProvider.ExecuteCmd(GetConnection, "dbo.OpenHouses_SelectByIdWithAppAgentDetail"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Id", id);

               }, map: delegate (IDataReader reader, short set)
               {

                   openHouse = MapOpenHouseAgentDetailReader(reader);
               }
               );

            return openHouse;
        }
       
        public List<OpenHouseAgentDetail> GetAgentOpenHousesByDateRange(OpenHouseDatesByAgentDetailRequest model)
        {
            List<OpenHouseAgentDetail> openHouseList = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.OpenHouses_SelectByAgentAndDates"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@AgentPersonId", model.AgentPersonId);
                   paramCollection.AddWithValue("@StartDate", model.StartDate);
                   if (model.EndDate.HasValue && (model.EndDate > model.StartDate))
                   {
                       paramCollection.AddWithValue("@EndDate", model.EndDate);
                   }
               }, map: delegate (IDataReader reader, short set)
               {
                   OpenHouseAgentDetail agentOpenHouse = MapOpenHouseAgentDetailReader(reader);

                   if (openHouseList == null)
                   {
                       openHouseList = new List<OpenHouseAgentDetail>();
                   }

                   openHouseList.Add(agentOpenHouse);
               }
               );

            return openHouseList;
        }

        private static OpenHouseBase MapOpenHouseReader(IDataReader reader)
        {
            OpenHouseBase openHouse = new OpenHouseBase();

            int ordinalIndex = 0;

            openHouse.Id = reader.GetSafeInt32(ordinalIndex++);
			openHouse.ListingId = reader.GetSafeInt32(ordinalIndex++);
            openHouse.MlsListingId = reader.GetSafeString(ordinalIndex++);
		    openHouse.ListPrice = reader.GetSafeInt32Nullable(ordinalIndex++);
            openHouse.StandardStatusName = reader.GetSafeString(ordinalIndex++);
		    openHouse.PropertySubTypeName = reader.GetSafeString(ordinalIndex++);
		    openHouse.YearBuilt = reader.GetSafeInt32Nullable(ordinalIndex++);
            openHouse.StreetName = reader.GetSafeString(ordinalIndex++);
		    openHouse.StreetSuffix = reader.GetSafeString(ordinalIndex++);
		    openHouse.StreetAddress = reader.GetSafeString(ordinalIndex++);
		    openHouse.UnitNumber = reader.GetSafeString(ordinalIndex++);
		    openHouse.CityName = reader.GetSafeString(ordinalIndex++);
		    openHouse.StateOrProvince = reader.GetSafeString(ordinalIndex++);
		    openHouse.PostalCode = reader.GetSafeString(ordinalIndex++);
		    openHouse.LivingArea = reader.GetSafeInt32Nullable(ordinalIndex++);
            openHouse.LotSizeUnits = reader.GetSafeString(ordinalIndex++);
		    openHouse.LotSizeSquareFeet = reader.GetSafeDecimalNullable(ordinalIndex++);
            openHouse.BedroomsTotal = reader.GetSafeInt32Nullable(ordinalIndex++);
            openHouse.BathroomsTotalInteger = reader.GetSafeInt32Nullable(ordinalIndex++);
		    openHouse.BathroomsFull = reader.GetSafeInt32Nullable(ordinalIndex++);
		    openHouse.BathroomsThreeQuarter = reader.GetSafeInt32Nullable(ordinalIndex++);
		    openHouse.BathroomsHalf = reader.GetSafeInt32Nullable(ordinalIndex++);
		    openHouse.BathroomsOneQuarter = reader.GetSafeInt32Nullable(ordinalIndex++);
		    openHouse.ListAgentKeyNumeric = reader.GetSafeInt32Nullable(ordinalIndex++);
		    openHouse.ListAgentMlsId = reader.GetSafeString(ordinalIndex++);
		    openHouse.ListAgentStateLicense = reader.GetSafeString(ordinalIndex++);
		    openHouse.ListAgentFirstName = reader.GetSafeString(ordinalIndex++);
		    openHouse.ListAgentLastName = reader.GetSafeString(ordinalIndex++);
		    openHouse.ListOfficeKeyNumeric = reader.GetSafeInt32Nullable(ordinalIndex++);
		    openHouse.ListOfficeMlsId = reader.GetSafeString(ordinalIndex++);
		    openHouse.ListOfficeName = reader.GetSafeString(ordinalIndex++);
            openHouse.DefaultMediaURL = reader.GetSafeString(ordinalIndex++);
            openHouse.ListingKeyNumeric = reader.GetSafeInt32Nullable(ordinalIndex++);
            openHouse.OpenHouseKeyNumeric = reader.GetSafeInt32Nullable(ordinalIndex++);
		    openHouse.OpenHouseDate = reader.GetSafeDateTime(ordinalIndex++);
            openHouse.OpenHouseStartTimeNumeric = reader.GetSafeInt32Nullable(ordinalIndex++);
            openHouse.OpenHouseEndTimeNumeric = reader.GetSafeInt32Nullable(ordinalIndex++);
            openHouse.OpenHouseStartTime = reader[ordinalIndex++] as TimeSpan?;
            openHouse.OpenHouseEndTime = reader[ordinalIndex++] as TimeSpan?;
            openHouse.OpenHouseStatus = reader.GetSafeString(ordinalIndex++);
		    openHouse.OpenHouseStatusName = reader.GetSafeString(ordinalIndex++);
		    openHouse.OpenHouseType = reader.GetSafeString(ordinalIndex++);
		    openHouse.OpenHouseTypeName = reader.GetSafeString(ordinalIndex++);
		    openHouse.OpenHouseDirections = reader.GetSafeString(ordinalIndex++);
		    openHouse.OpenHouseRemarks = reader.GetSafeString(ordinalIndex++);
		    openHouse.OpenHouseAttendedBy = reader.GetSafeString(ordinalIndex++);
		    openHouse.Refreshments = reader.GetSafeString(ordinalIndex++);
		    openHouse.DrawingYN = reader.GetSafeBool(ordinalIndex++);
		    openHouse.DeletedYN = reader.GetSafeBool(ordinalIndex++);
		    openHouse.VisitorCount = reader.GetSafeInt32(ordinalIndex++);

            return openHouse;
        }

        private static OpenHouseAgentDetail MapOpenHouseAgentDetailReader(IDataReader reader)
        {
            OpenHouseAgentDetail agentOpenHouse = new OpenHouseAgentDetail();

            int ordinalIndex = 0;

            agentOpenHouse.AgentPersonId = reader.GetSafeInt32(ordinalIndex++);
            agentOpenHouse.AgentFirstName = reader.GetSafeString(ordinalIndex++);
            agentOpenHouse.AgentLastName = reader.GetSafeString(ordinalIndex++);
            agentOpenHouse.AgentPhoneWork = reader.GetSafeString(ordinalIndex++);
            agentOpenHouse.AgentEmailPublic = reader.GetSafeString(ordinalIndex++);
            agentOpenHouse.AgentCompanyName = reader.GetSafeString(ordinalIndex++);
            agentOpenHouse.AgentAddress1 = reader.GetSafeString(ordinalIndex++);
            agentOpenHouse.AgentAddress2 = reader.GetSafeString(ordinalIndex++);
            agentOpenHouse.AgentCity = reader.GetSafeString(ordinalIndex++);
            agentOpenHouse.AgentStateProvice = reader.GetSafeString(ordinalIndex++);
            agentOpenHouse.AgentZipCode = reader.GetSafeString(ordinalIndex++);
            agentOpenHouse.AgentWebSite = reader.GetSafeString(ordinalIndex++);
            agentOpenHouse.AgentMlsId = reader.GetSafeString(ordinalIndex++);
            agentOpenHouse.AgentLowResKeyName = reader.GetSafeString(ordinalIndex++);
            agentOpenHouse.AgentHighResKeyName = reader.GetSafeString(ordinalIndex++);
            //from openhouse base
            agentOpenHouse.Id = reader.GetSafeInt32(ordinalIndex++);
            agentOpenHouse.ListingId = reader.GetSafeInt32(ordinalIndex++);
            agentOpenHouse.MlsListingId = reader.GetSafeString(ordinalIndex++);
		    agentOpenHouse.ListPrice = reader.GetSafeInt32Nullable(ordinalIndex++);
            agentOpenHouse.StandardStatusName = reader.GetSafeString(ordinalIndex++);
		    agentOpenHouse.PropertySubTypeName = reader.GetSafeString(ordinalIndex++);
		    agentOpenHouse.YearBuilt = reader.GetSafeInt32Nullable(ordinalIndex++);
            agentOpenHouse.StreetName = reader.GetSafeString(ordinalIndex++);
		    agentOpenHouse.StreetSuffix = reader.GetSafeString(ordinalIndex++);
		    agentOpenHouse.StreetAddress = reader.GetSafeString(ordinalIndex++);
            agentOpenHouse.UnitNumber = reader.GetSafeString(ordinalIndex++);
		    agentOpenHouse.CityName = reader.GetSafeString(ordinalIndex++);
		    agentOpenHouse.StateOrProvince = reader.GetSafeString(ordinalIndex++);
		    agentOpenHouse.PostalCode = reader.GetSafeString(ordinalIndex++);
		    agentOpenHouse.LivingArea = reader.GetSafeInt32Nullable(ordinalIndex++);
            agentOpenHouse.LotSizeUnits = reader.GetSafeString(ordinalIndex++);
		    agentOpenHouse.LotSizeSquareFeet = reader.GetSafeDecimalNullable(ordinalIndex++);
            agentOpenHouse.BedroomsTotal = reader.GetSafeInt32Nullable(ordinalIndex++);
            agentOpenHouse.BathroomsTotalInteger = reader.GetSafeInt32Nullable(ordinalIndex++);
		    agentOpenHouse.BathroomsFull = reader.GetSafeInt32Nullable(ordinalIndex++);
		    agentOpenHouse.BathroomsThreeQuarter = reader.GetSafeInt32Nullable(ordinalIndex++);
		    agentOpenHouse.BathroomsHalf = reader.GetSafeInt32Nullable(ordinalIndex++);
		    agentOpenHouse.BathroomsOneQuarter = reader.GetSafeInt32Nullable(ordinalIndex++);
		    agentOpenHouse.ListAgentKeyNumeric = reader.GetSafeInt32Nullable(ordinalIndex++);
		    agentOpenHouse.ListAgentMlsId = reader.GetSafeString(ordinalIndex++);
		    agentOpenHouse.ListAgentStateLicense = reader.GetSafeString(ordinalIndex++);
		    agentOpenHouse.ListAgentFirstName = reader.GetSafeString(ordinalIndex++);
		    agentOpenHouse.ListAgentLastName = reader.GetSafeString(ordinalIndex++);
		    agentOpenHouse.ListOfficeKeyNumeric = reader.GetSafeInt32Nullable(ordinalIndex++);
		    agentOpenHouse.ListOfficeMlsId = reader.GetSafeString(ordinalIndex++);
		    agentOpenHouse.ListOfficeName = reader.GetSafeString(ordinalIndex++);
            agentOpenHouse.DefaultMediaURL = reader.GetSafeString(ordinalIndex++);
            agentOpenHouse.ListingKeyNumeric = reader.GetSafeInt32Nullable(ordinalIndex++);
            agentOpenHouse.OpenHouseKeyNumeric = reader.GetSafeInt32Nullable(ordinalIndex++);
		    agentOpenHouse.OpenHouseDate = reader.GetSafeDateTime(ordinalIndex++);
            agentOpenHouse.OpenHouseStartTimeNumeric = reader.GetSafeInt32Nullable(ordinalIndex++);
            agentOpenHouse.OpenHouseEndTimeNumeric = reader.GetSafeInt32Nullable(ordinalIndex++);
            agentOpenHouse.OpenHouseStartTime = reader[ordinalIndex++] as TimeSpan?;
		    agentOpenHouse.OpenHouseEndTime = reader[ordinalIndex++] as TimeSpan?;
		    agentOpenHouse.OpenHouseStatus = reader.GetSafeString(ordinalIndex++);
		    agentOpenHouse.OpenHouseStatusName = reader.GetSafeString(ordinalIndex++);
		    agentOpenHouse.OpenHouseType = reader.GetSafeString(ordinalIndex++);
		    agentOpenHouse.OpenHouseTypeName = reader.GetSafeString(ordinalIndex++);
		    agentOpenHouse.OpenHouseDirections = reader.GetSafeString(ordinalIndex++);
		    agentOpenHouse.OpenHouseRemarks = reader.GetSafeString(ordinalIndex++);
		    agentOpenHouse.OpenHouseAttendedBy = reader.GetSafeString(ordinalIndex++);
		    agentOpenHouse.Refreshments = reader.GetSafeString(ordinalIndex++);
		    agentOpenHouse.DrawingYN = reader.GetSafeBool(ordinalIndex++);
		    agentOpenHouse.DeletedYN = reader.GetSafeBool(ordinalIndex++);
		    agentOpenHouse.VisitorCount = reader.GetSafeInt32(ordinalIndex++);
          
            return agentOpenHouse;
        }

        public int InsertVisitor(VisitorAddRequest model)
        {
            int Id = 0;
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.OpenHouseVisitors_Insert"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@openHouseId", model.OpenHouseId);
                    paramCollection.AddWithValue("@firstName", model.FirstName);
                    paramCollection.AddWithValue("@lastName", model.LastName);
                    paramCollection.AddWithValue("@email", model.Email);
                    paramCollection.AddWithValue("@phoneNumber", model.PhoneNumber);
                    paramCollection.AddWithValue("@lookingForAgent", model.LookingForAgent);
                    paramCollection.AddWithValue("@wantsUpdates", model.WantsUpdates);
                    paramCollection.AddWithValue("@sendSimilar", model.SendSimilar);
                    SqlParameter p = new SqlParameter("@id", System.Data.SqlDbType.Int);
                    p.Direction = System.Data.ParameterDirection.Output;
                    paramCollection.Add(p);
                }, returnParameters: delegate (SqlParameterCollection paramCollection)
                {
                    int.TryParse(paramCollection["@id"].Value.ToString(), out Id);
                });
            return Id;
        }

        public void UpdateVisitor(VisitorUpdateRequest model)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.OpenHouseVisitors_Update"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@id", model.Id);
                    paramCollection.AddWithValue("@firstName", model.FirstName);
                    paramCollection.AddWithValue("@lastName", model.LastName);
                    paramCollection.AddWithValue("@email", model.Email);
                    paramCollection.AddWithValue("@phoneNumber", model.PhoneNumber);
                    paramCollection.AddWithValue("@lookingForAgent", model.LookingForAgent);
                    paramCollection.AddWithValue("@wantsUpdates", model.WantsUpdates);
                    paramCollection.AddWithValue("@sendSimilar", model.SendSimilar);
                }, returnParameters: null);
        }

        public List<OpenHouseVisitor> GetVisitorsByAgentId(int Id)
        {
            List<OpenHouseVisitor> list = null;
            DataProvider.ExecuteCmd(GetConnection, "dbo.OpenHouseVisitors_SelectByAgentId"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@agentId", Id);
                }
                ,map: delegate (IDataReader reader, short set)
                {
                    OpenHouseVisitor v = new OpenHouseVisitor();
                    int startingIndex = 0;
                    v.Id = reader.GetSafeInt32(startingIndex++);
                    v.OpenHouseId = reader.GetSafeInt32(startingIndex++);
                    v.FirstName = reader.GetSafeString(startingIndex++);
                    v.LastName = reader.GetSafeString(startingIndex++);
                    v.Email = reader.GetSafeString(startingIndex++);
                    v.PhoneNumber = reader.GetSafeString(startingIndex++);
                    v.LookingForAgent = reader.GetSafeBool(startingIndex++);
                    v.WantsUpdates = reader.GetSafeBool(startingIndex++);
                    v.SendSimilar = reader.GetSafeBool(startingIndex++);
                    v.OpenHouseDate = reader.GetSafeDateTime(startingIndex++);
                    v.StreetAddress = reader.GetSafeString(startingIndex++);

                    if (list == null)
                    {
                        list = new List<OpenHouseVisitor>();
                    }
                    list.Add(v);
                });
            return list;
        }

        public List<Visitor> SelectVisitorsByOpenHouseId(int Id)
        {
            List<Visitor> list = null;
            DataProvider.ExecuteCmd(GetConnection, "dbo.OpenHouseVisitors_SelectByOpenHouseId"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@openHouseId", Id);
                }, map: delegate (IDataReader reader, short set)
                {
                    Visitor v = new Visitor();
                    int startingIndex = 0;
                    v.Id = reader.GetSafeInt32(startingIndex++);
                    v.OpenHouseId = reader.GetSafeInt32(startingIndex++);
                    v.FirstName = reader.GetSafeString(startingIndex++);
                    v.LastName = reader.GetSafeString(startingIndex++);
                    v.Email = reader.GetSafeString(startingIndex++);
                    v.PhoneNumber = reader.GetSafeString(startingIndex++);
                    v.LookingForAgent = reader.GetSafeBool(startingIndex++);
                    v.WantsUpdates = reader.GetSafeBool(startingIndex++);
                    v.SendSimilar = reader.GetSafeBool(startingIndex++);

                    if (list == null)
                    {
                        list = new List<Visitor>();
                    }
                    list.Add(v);
                });
            return list;
        }

        public void Unsubscribe(int Id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.OpenHouseVisitors_UpdateUnsubscribe"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@id", Id);
                }, returnParameters: null);
        }

    }
}
 
 