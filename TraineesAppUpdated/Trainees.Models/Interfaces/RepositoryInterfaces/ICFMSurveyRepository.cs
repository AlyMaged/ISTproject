using Trainees.Models.Interfaces.Base;
using Trainees.Models.Models;
using System.Collections.Generic;

namespace Trainees.Models.Interfaces.RepositoryInterfaces
{
    public interface ICFMSurveyRepository : IRepository<CFMSurvey>
    {
        List<CFMSurvey> GetWithUsers();
        List<CFMSurvey> GetSurveyForUsers(int userid);
    }
}
