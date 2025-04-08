namespace DDBCombatSim.GameSystem.Utils;

public enum ETestResult
{
    CriticalSuccess,
    CriticalFailure,
    Success,
    Failure
}

public static class ETestResultExtensions
{
    public static bool IsSuccess(this ETestResult testResult)
    {
        return testResult == ETestResult.Success || testResult == ETestResult.CriticalSuccess;
    }
    public static bool IsFailure(this ETestResult testResult)
    {
        return testResult == ETestResult.Failure || testResult == ETestResult.CriticalFailure;
    }
}