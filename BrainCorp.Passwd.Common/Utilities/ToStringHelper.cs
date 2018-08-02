using System;
using System.Text;
using System.Collections.Generic;


namespace BrainCorp.Passwd.Common.Utilities
{
    public class ToStringHelper
    {
        public ToStringHelper()
        {
            _sb = new StringBuilder();
        }

        public ToStringHelper BuildFormat(string format, object arg0)
        {
            _sb.AppendFormat(format, arg0);

            return this;
        }

        public ToStringHelper BuildFormat(string format, params object[] arg0)
        {
            _sb.AppendFormat(format, arg0);

            return this;
        }

        public ToStringHelper Build(string data)
        {
            _sb.Append(data);

            return this;
        }

        public ToStringHelper Build(string name, object value)
        {
            AppendProperty(name, value);

            return this;
        }

        public ToStringHelper Build<E>(string name, IEnumerable<E> values)
        {
            AppendProperty(name, values);

            return this;
        }

        public void AppendProperty(string name, object value)
        {
            _sb.Append(ParameterChecker.IsNull(name) + "=" + ParameterChecker.IsNull(value) + ", ");
        }
        public void AppendProperty(string name, string value)
        {
            _sb.Append(ParameterChecker.IsNull(name) + "=" + ParameterChecker.IsNull(value) + ", ");
        }
        public void AppendProperty<E>(string name, IEnumerable<E> values)
        {
            if (null == values)
            {
                _sb.Append(ParameterChecker.IsNull(name) + "=" + Environment.NewLine);
                _sb.Append(ParameterChecker.IsNull(values));
            }
            else
            {
                if (values is string ||
                    values is String)
                {
                    _sb.Append(ParameterChecker.IsNull(name) + "=" + ParameterChecker.IsNull(values));
                }
                else
                {
                    _sb.Append(ParameterChecker.IsNull(name) + "=" + Environment.NewLine);
                    foreach (E obj in values)
                    {
                        _sb.Append(ParameterChecker.IsNull(obj) + Environment.NewLine);
                    }
                }
            }

        }
        public void Append(object value)
        {
            _sb.Append(ParameterChecker.IsNullEmpty(value));
        }


        public void Append(string name, string value)
        {
            AppendProperty(ParameterChecker.IsNull(name), ParameterChecker.IsNull(value) + Environment.NewLine);
        }

        public override string ToString()
        {
            return _sb.ToString();
        }

        private StringBuilder _sb;
    }
}
