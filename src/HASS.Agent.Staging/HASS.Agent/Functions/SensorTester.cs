﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using HASS.Agent.Models.Internal;
using HASS.Agent.Shared.HomeAssistant.Sensors;
using Serilog;

namespace HASS.Agent.Functions
{
    internal static class SensorTester
    {
        /// <summary>
        /// Creates a WMI sensor with the provided values and gets the corresponding state
        /// </summary>
        /// <param name="query"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        internal static TestResult TestWmiQuery(string query, string scope = "", bool needRound = false, int round = 0)
        {
            try
            {
                // create a new sensor
                var wmiSensor = new WmiQuerySensor(query, scope, needRound, round);

                // get the state
                var value = wmiSensor.GetState();

                // dispose its searcher
                wmiSensor.Dispose();

                // done
                return new TestResult().SetSuccesful(value);
            }
            catch (Exception ex)
            {
                if (string.IsNullOrWhiteSpace(scope)) scope = @"\\localhost\";
                Log.Fatal(ex, "[SENSORTESTER] Error while testing WMI query\r\nScope: {scope}\r\nQuery: {query}\r\nError: {err}", scope, query, ex.Message);
                return new TestResult().SetFailed(ex.Message);
            }
        }

        /// <summary>
        /// Creates a PerformanceCounter sensor with the provided values and gets the corresponding state
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="counterName"></param>
        /// <param name="instanceName"></param>
        /// <returns></returns>
        internal static TestResult TestPerformanceCounter(string categoryName, string counterName, string instanceName, bool needRound = false, int? round = null)
        {
            try
            {
                // create a new sensor
                var performanceCounterSensor = new PerformanceCounterSensor(categoryName, counterName, instanceName);

                // get the state
                var value = performanceCounterSensor.GetState();

                // dispose its counter
                performanceCounterSensor.Dispose();

                // done
                return new TestResult().SetSuccesful(value);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "[SENSORTESTER] Error while testing PerformanceCounter\r\nCategory: {categoryName}\r\nCounter: {counterName}\r\nInstance: {instance}\r\nError: {err}", categoryName, counterName, instanceName, ex.Message);
                return new TestResult().SetFailed(ex.Message);
            }
        }

        /// <summary>
        /// Creates a Powershell sensor with the provided command and gets the result of its execution
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        internal static TestResult TestPowershell(string command, bool needRound = false, int? round = null)
        {
            try
            {
                // create a new sensor
                var powershellSensor = new PowershellSensor(command);

                // get the state
                var value = powershellSensor.GetState();
                
                // done
                return new TestResult().SetSuccesful(value);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "[SENSORTESTER] Error while testing Powershell\r\nCommand: {command}\r\nError: {err}", command, ex.Message);
                return new TestResult().SetFailed(ex.Message);
            }
        }
    }
}
