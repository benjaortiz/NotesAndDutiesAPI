using NotesAndDutiesAPI;
using NotesAndDutiesAPI.Models;

namespace Services.DutiesService;

public class DutiesService : IDutiesService
{

    private IDutiesRepository _duties;

    public DutiesService(IDutiesRepository duties)
    {
        _duties = duties;
    }

    public DutyModel AddDuty(PostDutyModel newDuty)
    {
        DutyModel duty = new DutyModel
        {
            Title = newDuty.Title,
            Status = newDuty.Status,
            Description = newDuty.Description
        };
        //should validate that the Id is correct before searching (not 0 or negative)
        DutyModel addedDuty = this._duties.addDuty(duty);

        return addedDuty;
    }

    public DutyModel AddDuty(PostDutyModel newDuty, string dutyAuthor)
    {
        DutyModel duty = new DutyModel
        {
            Title = newDuty.Title,
            Status = newDuty.Status,
            Description = newDuty.Description,
            Author = dutyAuthor.ToLower()
        };
        //should validate that the Id is correct before searching (not 0 or negative)
        DutyModel addedDuty = this._duties.addDuty(duty);

        return addedDuty;
    }

    public DutyModel? DeleteDuty(int id)
    {
        DutyModel? dutyToDelete = this._duties.GetDutyById(id);
        if (dutyToDelete != null)
        {
            this._duties.deleteDuty(id);
        }

        return dutyToDelete;
    }

    public DutyModel? DeleteDuty(int id, string user)
    {
        DutyModel? duty = this._duties.GetDutyById(id);

        if (duty != null && duty.Author.ToLower().Equals(user.ToLower()))
        {
            this._duties.deleteDuty(duty);
        }

        return duty;
    }

    public List<DutyModel> GetDuties()
    {
        return this._duties.GetDuties();
    }

    public List<DutyModel> GetDuties(string user)
    {
        return this._duties.GetDutiesByUsername(user.ToLower());
    }

    public DutyModel? GetDuty(int id)
    {
        return this._duties.GetDutyById(id);
    }

    public DutyModel? GetDuty(int id, string user)
    {
        DutyModel? duty = this._duties.GetDutyById(id);

        if (duty != null && duty.Author.ToLower().Equals(user.ToLower()))
        {
            return duty;
        }

        return null;
    }

    public DutyModel? ReplaceDuty(DutyModel updatedDuty)
    {
        return this._duties.replaceDuty(updatedDuty);
    }


    public DutyModel? ReplaceDuty(int id, DutyModelDTO updatedDuty)
    {
        //make the full dutymodel out of the DTO
        DutyModel duty = new DutyModel
        {
            DutyId = id,
            Title = updatedDuty.Title,
            Status = updatedDuty.Status,
            Description = updatedDuty.Description,
            Author = updatedDuty.Author
        };

        //search for the duty in the repository
        DutyModel? oldDuty = this._duties.GetDutyByIdAndUsername(id, duty.Author);

        if (oldDuty != null)
        {
            return this._duties.replaceDuty(duty);
        }

        return null;
    }
}