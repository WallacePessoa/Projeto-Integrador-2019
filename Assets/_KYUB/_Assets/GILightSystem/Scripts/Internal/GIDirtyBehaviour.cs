/*
Copyright 2019 KYUB INTERACTIVE LTDA (http://www.kyubinteractive.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kyub.GI
{
    public abstract class GIDirtyBehaviour : MonoBehaviour
    {
        #region Unity Functions

        protected virtual void OnEnable()
        {
            if (_started)
                TryApply(true);
        }

        protected virtual void OnDisable()
        {
            TryFixRoutineState();
        }

        protected bool _started = false;
        protected virtual void Start()
        {
            _started = true;
            TryApply(true);
        }

        #endregion

        #region Helper Functions

        protected bool _isDirty = false;
        public virtual void SetDirty()
        {
            _isDirty = true;
            TryFixRoutineState();
        }

        public virtual void TryApply(bool p_force = false)
        {
            if (_isDirty || p_force)
            {
                _isDirty = false;
                Apply();
            }

            TryFixRoutineState();
        }

        protected virtual void Apply()
        {
        }

        #endregion

        #region Routine Checker

        protected virtual void TryFixRoutineState()
        {
            if (this != null)
            {
                var v_isValid = this.gameObject.activeInHierarchy && enabled && _isDirty;

                //Try stop running routines if state is invalid
                if (_applyRoutineController != null && !v_isValid)
                {
                    StopCoroutine(_applyRoutineController);
                    _applyRoutineController = null;
                }

                //Start routine if is valid
                if (v_isValid)
                {
                    if (_applyRoutineController == null)
                        _applyRoutineController = this.StartCoroutine(TryApply_TickRoutine());
                }
                else
                    _applyRoutineController = null;
            }
        }

        protected Coroutine _applyRoutineController = null;
        protected virtual IEnumerator TryApply_TickRoutine()
        {
            while (_isDirty)
            {
                yield return null;
                TryApply();
            }
            _applyRoutineController = null;
        }

        #endregion
    }
}
