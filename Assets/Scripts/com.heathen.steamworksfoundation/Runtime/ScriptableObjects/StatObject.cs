﻿#if !DISABLESTEAMWORKS && HE_SYSCORE && STEAMWORKSNET
using Steamworks;
using System;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration
{
    /// <summary>
    /// A <see cref="ScriptableObject"/> containing the definition of a Steamworks Stat.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Note that this object simply contains the definition of a stat that has been created in the Steamworks API.
    /// for more information please see <a href="https://partner.steamgames.com/doc/features/achievements">https://partner.steamgames.com/doc/features/achievements</a>
    /// </para>
    /// </remarks>
    [HelpURL("https://kb.heathenengineering.com/assets/steamworks/stats-object")]
    [Serializable]
    public abstract class StatObject : ScriptableObject
    {
        /// <summary>
        /// The name of the stat as it appears in the Steamworks Portal
        /// </summary>
        [HideInInspector]
        public StatData data;
        /// <summary>
        /// Indicates the data type of this stat.
        /// This is used when working with the generic <see cref="StatObject"/> reference.
        /// </summary>
        public abstract DataType Type { get; }
        /// <summary>
        /// Returns the value of this stat as an int.
        /// This is used when working with the generic <see cref="StatObject"/> reference.
        /// </summary>
        /// <returns></returns>
        public int GetIntValue() => data.IntValue();
        /// <summary>
        /// Returns the value of this stat as a float.
        /// This is used when working with the generic <see cref="StatObject"/> reference.
        /// </summary>
        /// <returns></returns>
        public float GetFloatValue() => data.FloatValue();
        /// <summary>
        /// Sets the value of this stat on the Steamworks API.
        /// This is used when working with the generic <see cref="StatObject"/> reference.
        /// </summary>
        /// <param name="value">The value to set on the API</param>
        public void SetIntStat(int value) => data.Set(value);
        /// <summary>
        /// Sets the value of this stat on the Steamworks API.
        /// This is used when working with the generic <see cref="StatObject"/> reference.
        /// </summary>
        /// <param name="value">The value to set on the API</param>
        public void SetFloatStat(float value) => data.Set(value);
        /// <summary>
        /// Adds the provided value to the existing value of the stat
        /// </summary>
        /// <param name="value">The value to add to the current value</param>
        public void AddFloatStat(float value)
        {
            SetFloatStat(GetFloatValue() + value);
        }
        /// <summary>
        /// Adds the provided value to the existing value of the stat
        /// </summary>
        /// <param name="value">The value to add to the current value</param>
        public void AddIntStat(int value)
        {
            SetIntStat(GetIntValue() + value);
        }
        /// <summary>
        /// This stores all stats to the Valve backend servers it is not possible to store only 1 stat at a time
        /// Note that this will cause a callback from Steamworks which will cause the stats to update
        /// </summary>
        public void StoreStats() => StatData.Store();
        /// <summary>
        /// The availble type of stat data used in the Steamworks API
        /// </summary>
        public enum DataType
        {
            Int,
            Float,
            AvgRate
        }

#if UNITY_SERVER || UNITY_EDITOR
        /// <summary>
        /// Get the int value of this stat for the <paramref name="user"/>
        /// </summary>
        /// <remarks>
        /// <para>
        /// IMPORTANT: you must first call <see cref="SteamworksClientApiSettings.GameServer.RequestUserStats(CSteamID, Action{GSStatsReceived_t})"/> via SteamSettings.current.server.RequestUserStats(id, callbackMethod);
        /// </para>
        /// <para>
        /// Only available on server builds
        /// </para>
        /// </remarks>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetUserIntStat(CSteamID user)
        {
            int buffer;
            SteamGameServerStats.GetUserStat(user, data, out buffer);
            return buffer;
        }

        /// <summary>
        /// Get the float value of this stat for the <paramref name="user"/>
        /// </summary>
        /// <remarks>
        /// <para>
        /// IMPORTANT: you must first call <see cref="SteamworksClientApiSettings.GameServer.RequestUserStats(CSteamID, Action{GSStatsReceived_t})"/> via SteamSettings.current.server.RequestUserStats(id, callbackMethod);
        /// </para>
        /// <para>
        /// Only available on server builds
        /// </para>
        /// </remarks>
        /// <param name="user"></param>
        /// <returns></returns>
        public float GetUserFloatStat(CSteamID user)
        {
            float buffer;
            SteamGameServerStats.GetUserStat(user, data, out buffer);
            return buffer;
        }

        /// <summary>
        /// Sets a integer value for the user on this stat
        /// </summary>
        /// <remarks>
        /// <para>
        /// Only available on server builds
        /// </para>
        /// </remarks>
        /// <param name="user"></param>
        /// <param name="value"></param>
        public void SetUserIntStat(CSteamID user, int value)
        {
            SteamGameServerStats.SetUserStat(user, data, value);
        }

        /// <summary>
        /// Sets a float value for the user on this stat
        /// </summary>
        /// <remarks>
        /// <para>
        /// Only available on server builds
        /// </para>
        /// </remarks>
        /// <param name="user"></param>
        /// <param name="value"></param>
        public void SetUserFloatStat(CSteamID user, float value)
        {
            SteamGameServerStats.SetUserStat(user, data, value);
        }

        /// <summary>
        /// Updates the users average rate for this stat
        /// </summary>
        /// <remarks>
        /// <para>
        /// Only available on server builds
        /// </para>
        /// </remarks>
        /// <param name="user"></param>
        /// <param name="countThisSession"></param>
        /// <param name="sessionLength"></param>
        public void UpdateUserAvgRateStat(CSteamID user, float countThisSession, double sessionLength)
        {
            SteamGameServerStats.UpdateUserAvgRateStat(user, data, countThisSession, sessionLength);
        }
#endif
    }
}
#endif
