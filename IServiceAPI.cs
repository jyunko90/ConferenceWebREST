using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ConferenceRESTSystem
{
    public interface IServiceAPI
    {
        //void CreateNewAccount(String firstName, String lastName, String username, String password);
        DataTable getSignupOption(); // get Title, Gender and Country option for signup
        bool signup(String Email, String Username, String TitleId, String FullName, String GenderId, String Instituition, String Faculty, String Department, String ResearchField, String Address, String State, String PostalCode, String CountryId, String PhoneNumber, String FaxNumber, String encryptedPassword);
        long login(String Username, String encryptedPassword);
        DataTable getUserDetail(String userId);

        DataTable getEvents();

        DataTable getRegisterEventOption(String conferenceId); // get Fee and UserType option used to register event
        bool registerEvents(String conferenceId, String feeId, String userId, String userTypeId);
    }
}