using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Analytics;
using RGN.Attributes;
using RGN.ImplDependencies.Core;

namespace RGN.Modules.Analytics.Runtime
{
    [InjectImplDependency(typeof(IAnalytics))]
    public sealed class Analytics : IAnalytics
    {
        private readonly List<Parameter> mLogEventParams = new List<Parameter>();
        private bool _disabled;

        public Task<string> GetAnalyticsInstanceIdAsync()
        {
            return FirebaseAnalytics.GetAnalyticsInstanceIdAsync();
        }
        public void Init()
        {
        }
        public void Dispose()
        {
            mLogEventParams.Clear();
        }
        public void DisableUserTracking()
        {
            _disabled = true;
            RGNCore.I.Dependencies.Logger.LogWarning("Analytics user tracking disabled");
        }
        public void LogEvent(string name)
        {
            if (_disabled)
            {
                return;
            }
            FirebaseAnalytics.LogEvent(name);
        }
        public void LogEvent(string name, string parameterName, long parameterValue)
        {
            if (_disabled)
            {
                return;
            }
            FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
        }
        public void LogEvent(string name, string parameterName, string parameterValue)
        {
            if (_disabled)
            {
                return;
            }
            FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
        }
        public void LogEvent(string name, string parameterName, float parameterValue)
        {
            if (_disabled)
            {
                return;
            }
            FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
        }
        public void LogEvent(
            string name,
            string parameterNameOne,
            string parameterValueOne,

            string parameterNameTwo,
            long parameterValueTwo)
        {
            if (_disabled)
            {
                return;
            }
            mLogEventParams.Clear();
            AddNewParameterToParametersArguments(new AnalyticsParameter(parameterNameOne, parameterValueOne));
            AddNewParameterToParametersArguments(new AnalyticsParameter(parameterNameTwo, parameterValueTwo));
            FirebaseAnalytics.LogEvent(name, mLogEventParams.ToArray());
        }
        public void LogEvent(
            string name,
            string parameterNameOne,
            string parameterValueOne,

            string parameterNameTwo,
            string parameterValueTwo)
        {
            if (_disabled)
            {
                return;
            }
            mLogEventParams.Clear();
            AddNewParameterToParametersArguments(new AnalyticsParameter(parameterNameOne, parameterValueOne));
            AddNewParameterToParametersArguments(new AnalyticsParameter(parameterNameTwo, parameterValueTwo));
            FirebaseAnalytics.LogEvent(name, mLogEventParams.ToArray());
        }
        public void LogEvent(
            string name,
            string parameterNameOne,
            string parameterValueOne,

            string parameterNameTwo,
            float parameterValueTwo)
        {
            if (_disabled)
            {
                return;
            }
            mLogEventParams.Clear();
            AddNewParameterToParametersArguments(new AnalyticsParameter(parameterNameOne, parameterValueOne));
            AddNewParameterToParametersArguments(new AnalyticsParameter(parameterNameTwo, parameterValueTwo));
            FirebaseAnalytics.LogEvent(name, mLogEventParams.ToArray());
        }
        public void LogEvent(
            string name,
            string parameterNameOne,
            string parameterValueOne,

            string parameterNameTwo,
            double parameterValueTwo)
        {
            if (_disabled)
            {
                return;
            }
            mLogEventParams.Clear();
            AddNewParameterToParametersArguments(new AnalyticsParameter(parameterNameOne, parameterValueOne));
            AddNewParameterToParametersArguments(new AnalyticsParameter(parameterNameTwo, parameterValueTwo));
            FirebaseAnalytics.LogEvent(name, mLogEventParams.ToArray());
        }
        public void LogEvent(
            string name,
            params AnalyticsParameter[] analyticParameters)
        {
            if (_disabled)
            {
                return;
            }
            mLogEventParams.Clear();
            for (int i = 0; i < analyticParameters.Length; ++i)
            {
                AnalyticsParameter param = analyticParameters[i];
                AddNewParameterToParametersArguments(param);
            }
            FirebaseAnalytics.LogEvent(name, mLogEventParams.ToArray());
        }
        public void LogExceptionEvent(string exceptionMessage)
        {
            if (_disabled)
            {
                return;
            }
            FirebaseAnalytics.LogEvent("exception", "message", exceptionMessage);
        }

        public void SetCurrentScreen(string screenName, string screenClass)
        {
            if (_disabled)
            {
                return;
            }
            FirebaseAnalytics.LogEvent(
                FirebaseAnalytics.EventScreenView,
                FirebaseAnalytics.ParameterScreenName,
                screenName);
        }
        public void SetUserId(string userId)
        {
            if (_disabled)
            {
                return;
            }
            FirebaseAnalytics.SetUserId(userId);
            RGNCore.I.Dependencies.Logger.LogWarning("Analytics User Id set to " + userId);
        }
        public void TutorialBegin()
        {
            if (_disabled)
            {
                return;
            }
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventTutorialBegin);
        }
        public void TutorialComplete()
        {
            if (_disabled)
            {
                return;
            }
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventTutorialComplete);
        }
        public void Login()
        {
            if (_disabled)
            {
                return;
            }
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
        }

        private void AddNewParameterToParametersArguments(AnalyticsParameter param)
        {
            if (param.ParameterValue is double)
            {
                mLogEventParams.Add(new Parameter(param.ParameterName, (double)param.ParameterValue));
            }
            else if (param.ParameterValue is float)
            {
                mLogEventParams.Add(new Parameter(param.ParameterName, (float)param.ParameterValue));
            }
            else if (param.ParameterValue is long)
            {
                mLogEventParams.Add(new Parameter(param.ParameterName, (long)param.ParameterValue));
            }
            else if (param.ParameterValue is int)
            {
                mLogEventParams.Add(new Parameter(param.ParameterName, (int)param.ParameterValue));
            }
            else if (param.ParameterValue is string)
            {
                mLogEventParams.Add(new Parameter(param.ParameterName, (string)param.ParameterValue));
            }
            else
            {
                throw new System.NotSupportedException($"The parameter type {param.ParameterValue.GetType()} is not supported");
            }
        }
    }
}
