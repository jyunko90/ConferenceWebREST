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
        bool signup(String email, String username, String password);
        long login(String username, String password);
        DataTable getUserDetail(String userId);

        DataTable getEvents();

        DataTable getRegisterEventOption(String conferenceId); // get Fee and UserType option used to register event
        bool registerEvents(String conferenceId, String feeId, String userId, String userTypeId);
    }
}