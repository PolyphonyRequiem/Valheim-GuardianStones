using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GuardianStones.MonoBehaviors
{
    [AddComponentMenu("Augments/GuardianStone", 1)]
    [Serializable]
    [DisallowMultipleComponent]
    public class GuardianStone : MonoBehaviour
    {
        private static List<GuardianStone> all_GuardianStones = new List<GuardianStone>();

        private ZNetView m_nview;

        [Header("Configure")]
        [SerializeField, Tooltip("The effective range of the Guardian Stone's boon and construction field.")]
        private float effectRange = 10;

        private GuardianStone()
        {

        }

        private void Awake()
        {

        }

        private void Start()
        {
            m_nview = GetComponent<ZNetView>();

            if (!m_nview || m_nview.GetZDO() != null)
            {
                all_GuardianStones.Add(this);
            }
        }

        public static bool IsNearby(Vector3 position) => all_GuardianStones.Any(s => Vector3.Distance(position, s.transform.position) < s.effectRange);


        private void OnDestroy()
        {
            all_GuardianStones.Remove(this);
        }
    }
}
