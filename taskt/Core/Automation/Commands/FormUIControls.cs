using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static taskt.UI.CustomControls.CommandControls;

namespace taskt.Core.Automation.Commands
{
    public static class FormUIControls
    {
        #region control search method

        /// <summary>
        /// get parameter group controls
        /// </summary>
        /// <param name="ctrls"></param>
        /// <param name="parameterName"></param>
        /// <param name="nextParameterName"></param>
        /// <returns></returns>
        public static List<Control> GetControlGroup(this List<Control> ctrls, string parameterName, string nextParameterName = "")
        {
            List<Control> ret = new List<Control>();

            int index;
            index = ctrls.FindIndex(t => (t.Name == GROUP_PREFIX + parameterName));
            if (index >= 0)
            {
                ret.Add(ctrls[index]);
            }
            else
            {
                index = ctrls.FindIndex(t => (t.Name == LABEL_PREFIX + parameterName));
                int last = (nextParameterName == "") ? ctrls.Count : ctrls.FindIndex(t => (t.Name == LABEL_PREFIX + nextParameterName));

                for (int i = index; i < last; i++)
                {
                    ret.Add(ctrls[i]);
                }
            }

            return ret;
        }

        /// <summary>
        /// get paramter body control
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controls"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T GetPropertyControl<T>(this Dictionary<string, Control> controls, string parameterName) where T : Control
        {
            if (controls.ContainsKey(parameterName))
            {
                return (T)controls[parameterName];
            }
            else
            {
                throw new Exception("Control '" + parameterName + "' does not exists.");
            }
        }

        /// <summary>
        /// get paramter label
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Label GetPropertyControlLabel(this Dictionary<string, Control> controls, string parameterName)
        {
            if (controls.ContainsKey(LABEL_PREFIX + parameterName))
            {
                return (Label)controls[LABEL_PREFIX + parameterName];
            }
            else
            {
                throw new Exception("Label '" + LABEL_PREFIX + parameterName + "' does not exists.");
            }
        }

        /// <summary>
        /// get parameter 2nd label
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Label GetPropertyControl2ndLabel(this Dictionary<string, Control> controls, string parameterName)
        {
            if (controls.ContainsKey(LABEL_2ND_PREFIX + parameterName))
            {
                return (Label)controls[LABEL_2ND_PREFIX + parameterName];
            }
            else
            {
                throw new Exception("2nd Label '" + LABEL_2ND_PREFIX + parameterName + "' does not exists.");
            }
        }

        /// <summary>
        /// get all paramter controls
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controls"></param>
        /// <param name="parameterName"></param>
        /// <param name="throwWhenLabelNotExists"></param>
        /// <param name="throwWhen2ndLabelNotExists"></param>
        /// <returns></returns>
        public static (T body, Label label, Label label2nd) GetAllPropertyControl<T>(this Dictionary<string, Control> controls, string parameterName, bool throwWhenLabelNotExists = true, bool throwWhen2ndLabelNotExists = false) where T : Control
        {
            T body = controls.GetPropertyControl<T>(parameterName);

            Label label;
            try
            {
                label = controls.GetPropertyControlLabel(parameterName);
            }
            catch (Exception ex)
            {
                if (throwWhenLabelNotExists)
                {
                    throw ex;
                }
                else
                {
                    label = null;
                }
            }
            Label label2nd;
            try
            {
                label2nd = controls.GetPropertyControl2ndLabel(parameterName);
            }
            catch (Exception ex)
            {
                if (throwWhen2ndLabelNotExists)
                {
                    throw ex;
                }
                else
                {
                    label2nd = null;
                }
            }
            return (body, label, label2nd);
        }

        /// <summary>
        /// get 2nd label text
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Get2ndLabelText(this Dictionary<string, Control> controls, string parameterName)
        {
            return controls.GetPropertyControlLabel(parameterName).Get2ndLabelTexts();
        }

        /// <summary>
        /// get 2nd label text
        /// </summary>
        /// <param name="lbl"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Dictionary<string, string> Get2ndLabelTexts(this Label lbl)
        {
            if (lbl.Tag is Dictionary<string, string> dic)
            {
                return dic;
            }
            else
            {
                throw new Exception(lbl.Name + " does not has Dictionary item for 2nd-Label");
            }
        }

        /// <summary>
        /// create 2nd label text
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="labelTextName"></param>
        /// <param name="label2ndName"></param>
        /// <param name="key"></param>
        public static void SecondLabelProcess(this Dictionary<string, Control> controls, string labelTextName, string label2ndName, string key)
        {
            var dic = controls.Get2ndLabelText(labelTextName);
            var lbl = controls.GetPropertyControl2ndLabel(label2ndName);

            lbl.Text = dic.ContainsKey(key) ? dic[key] : "";
        }


        /// <summary>
        /// show/hide Command parameter groups
        /// </summary>
        /// <param name="controlsList"></param>
        /// <param name="parameterName"></param>
        /// <param name="visible"></param>
        public static void SetVisibleParameterControlGroup(Dictionary<string, Control> controlsList, string parameterName, bool visible)
        {
            foreach (var ctrl in controlsList)
            {
                if (ctrl.Key.Contains(parameterName))
                {
                    ctrl.Value.Visible = visible;
                }
            }
        }
        #endregion
    }
}
