using ProjectB.Exceptions;

namespace ProjectB.Models;

public class TourCompositeKey {

    public readonly DateTime Time;
    public Employee Guide { get; set; }

    public TourCompositeKey(DateTime time, Employee guide) {
        SetGuide(guide);
        Time = time;
    }

    public void SetGuide(Employee guide) {
        if (guide.GetUserRole() != UserRole.Guide) {
            throw new PrimaryKeyConstraintException("Employee assigned to tour must be a Guide.");
        }
        Guide = guide;
    }
}