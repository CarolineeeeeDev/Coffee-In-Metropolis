﻿namespace Zinnia.Tracking.CameraRig
{
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.XR;
    using Zinnia.Process;

    public class DominantControllerObserver : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// Defines the event with the <see cref="DeviceDetailsRecord"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<DeviceDetailsRecord> { }

        #region Reference Settings
        /// <summary>
        /// The details about the left controller.
        /// </summary>
        [Serialized, Cleared]
        [field: Header("Controller Settings"), DocumentedByXml]
        public DeviceDetailsRecord LeftController { get; set; }
        /// <summary>
        /// The details about the right controller.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public DeviceDetailsRecord RightController { get; set; }
        #endregion

        #region Device Events
        /// <summary>
        /// Emitted as the dominant controller is changing.
        /// </summary>
        [Header("Device Events"), DocumentedByXml]
        public UnityEvent IsChanging = new UnityEvent();
        #endregion

        /// <summary>
        /// The current dominant controller node.
        /// </summary>
        public virtual XRNode DominantController => GetDominantController() == null ? XRNode.Head : LastKnownDominantControllerDetails.XRNodeType;
        /// <summary>
        /// The current dominant controller.
        /// </summary>
        public virtual DeviceDetailsRecord DominantControllerDetails => GetDominantController();
        /// <summary>
        /// The last known dominant controller without doing a fresh query.
        /// </summary>
        public virtual DeviceDetailsRecord LastKnownDominantControllerDetails { get; protected set; }

        /// <summary>
        /// Processes the state of the dominant controller.
        /// </summary>
        public virtual void Process()
        {
            GetDominantController();
        }

        /// <summary>
        /// Gets the dominant controller.
        /// </summary>
        /// <returns>The dominant controller.</returns>
        protected virtual DeviceDetailsRecord GetDominantController()
        {
            DeviceDetailsRecord controller = null;
            if (!IsDeviceDetailsRecordConnected(RightController) && IsDeviceDetailsRecordConnected(LeftController))
            {
                controller = LeftController;
            }
            else if (IsDeviceDetailsRecordConnected(RightController) && !IsDeviceDetailsRecordConnected(LeftController))
            {
                controller = RightController;
            }
            else if (IsDeviceDetailsRecordConnected(RightController) && IsDeviceDetailsRecordConnected(LeftController))
            {
                controller = LeftController.Priority < RightController.Priority ? LeftController : RightController;
            }

            if (LastKnownDominantControllerDetails != controller)
            {
                IsChanging?.Invoke(controller);
            }

            LastKnownDominantControllerDetails = controller;

            return controller;
        }

        /// <summary>
        /// Determines whether the given device record is set and connected.
        /// </summary>
        /// <param name="record">The record to check.</param>
        /// <returns>Whether the given record is connected.</returns>
        protected virtual bool IsDeviceDetailsRecordConnected(DeviceDetailsRecord record)
        {
            return record != null && record.IsConnected;
        }
    }
}