using NUnit.Framework;
using UnityEngine;

namespace UnityMemoryNPCPrototype.Tests.EditMode
{
    /// <summary>
    /// Verifies the reproducible project environment baseline.
    /// </summary>
    public sealed class ProjectBaselineTests
    {
        /// <summary>
        /// Verifies that the current Unity Editor version matches the project requirement.
        /// </summary>
        [Test]
        public void UnityVersionMatchesProjectRequirement()
        {
            Assert.That(Application.unityVersion, Is.EqualTo(PrototypeEnvironment.RequiredUnityVersion));
        }
    }
}
