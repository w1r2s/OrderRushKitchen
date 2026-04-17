using Assets.Scripts.ScriptableObjects;

namespace Assets.Scripts.Order
{
    public interface IOrderSubmissionService
    {
        bool TrySubmit(MenuItemDefinitionSo menuItem, out OrderSubmissionFailureReason failureReason);
    }
}