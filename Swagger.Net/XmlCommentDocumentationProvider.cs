﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Xml.XPath;

namespace Swagger.Net
{
    /// <summary>
    /// Accesses the XML doc blocks written in code to further document the API.
    /// All credit goes to: <see cref="http://blogs.msdn.com/b/yaohuang1/archive/2012/05/21/asp-net-web-api-generating-a-web-api-help-page-using-apiexplorer.aspx"/>
    /// </summary>
    public class XmlCommentDocumentationProvider : IDocumentationProvider
    {
        XPathNavigator _documentNavigator;
        XPathNavigator _dataTypeMapNavigator;
        private const string _methodExpression = "/doc/members/member[@name='M:{0}']";
        private const string _returnsExpression = "/doc/members/member[@name='M:{0}']/returns";
<<<<<<< HEAD
=======
        private const string _responseCodeExpression = "/doc/members/member[@name='M:{0}']/response";
>>>>>>> origin/ibrahim
        private const string _dataTypeExpression = "/DataTypes/DataType[@Id='{0}']";
        private static Regex nullableTypeNameRegex = new Regex(@"(.*\.Nullable)" + Regex.Escape("`1[[") + "([^,]*),.*");
        private const string _newLine = "<br/>";

        public XmlCommentDocumentationProvider(string documentPath)
        {
            XPathDocument xpath = new XPathDocument(documentPath);
            _documentNavigator = xpath.CreateNavigator();
        }

        public virtual string GetDocumentation(HttpParameterDescriptor parameterDescriptor)
        {
            ReflectedHttpParameterDescriptor reflectedParameterDescriptor = parameterDescriptor as ReflectedHttpParameterDescriptor;
            if (reflectedParameterDescriptor != null)
            {
                XPathNavigator memberNode = GetMemberNode(reflectedParameterDescriptor.ActionDescriptor);
                if (memberNode != null)
                {
                    string parameterName = reflectedParameterDescriptor.ParameterInfo.Name;
                    XPathNavigator parameterNode = memberNode.SelectSingleNode(string.Format("param[@name='{0}']", parameterName));
                    if (parameterNode != null)
                    {
                        return parameterNode.Value.Trim();
                    }
                }
            }

            return "No Documentation Found.";
        }

        public virtual bool GetRequired(HttpParameterDescriptor parameterDescriptor)
        {
            ReflectedHttpParameterDescriptor reflectedParameterDescriptor = parameterDescriptor as ReflectedHttpParameterDescriptor;
            if (reflectedParameterDescriptor != null)
            {
                return !reflectedParameterDescriptor.ParameterInfo.IsOptional;
            }

            return true;
        }

        public virtual string GetDocumentation(HttpActionDescriptor actionDescriptor)
        {
            XPathNavigator memberNode = GetMemberNode(actionDescriptor);
            if (memberNode != null)
            {
                XPathNavigator summaryNode = memberNode.SelectSingleNode("summary");
                if (summaryNode != null)
                {
                    return summaryNode.Value.Trim();
                }
            }

            return "No Documentation Found.";
        }

        public virtual string GetNotes(HttpActionDescriptor actionDescriptor)
        {
            var responseCodesSummary = GetMemberResponseCodeSummary(actionDescriptor);
            var summary = string.Empty;
            XPathNavigator memberNode = GetMemberNode(actionDescriptor);
            if (memberNode != null)
            {
                XPathNavigator summaryNode = memberNode.SelectSingleNode("remarks");
                if (summaryNode != null)
                {
                    summary = string.Format("{0}{1}", summaryNode.Value.Trim(), _newLine);
                }
            }
            summary = string.Format("{0}{1}", summary, responseCodesSummary);
            return string.IsNullOrEmpty(summary) ? "No Documentation Found." : summary;
        }

        public virtual string GetResponseClass(HttpActionDescriptor actionDescriptor)
        {
            ReflectedHttpActionDescriptor reflectedActionDescriptor = actionDescriptor as ReflectedHttpActionDescriptor;
            if (reflectedActionDescriptor != null)
            {
                if (reflectedActionDescriptor.MethodInfo.ReturnType.IsGenericType)
                {
                    StringBuilder sb =
                        new StringBuilder(reflectedActionDescriptor.MethodInfo.ReturnParameter.ParameterType.Name);
                    sb.Append("<");
                    Type[] types =
                        reflectedActionDescriptor.MethodInfo.ReturnParameter.ParameterType.GetGenericArguments();
                    for (int i = 0; i < types.Length; i++)
                    {
                        sb.Append(types[i].Name);
                        if (i != (types.Length - 1)) sb.Append(", ");
                    }
                    sb.Append(">");
                    return sb.Replace("`1", "").ToString();
                }
                else
                {
                    var node = GetMemberReturnsNode(reflectedActionDescriptor);
                    if (node != null)
                    {
                        var type = node.GetAttribute("type", string.Empty);
                        if (!string.IsNullOrEmpty(type))
                            return type;
                    }
                    return reflectedActionDescriptor.MethodInfo.ReturnType.Name;
                }
            }

            return "void";
        }

        public virtual string GetNickname(HttpActionDescriptor actionDescriptor)
        {
            ReflectedHttpActionDescriptor reflectedActionDescriptor = actionDescriptor as ReflectedHttpActionDescriptor;
            if (reflectedActionDescriptor != null)
            {
                return reflectedActionDescriptor.MethodInfo.Name;
            }

            return "NicknameNotFound";
        }

        private XPathNavigator GetMemberNode(HttpActionDescriptor actionDescriptor)
        {
            ReflectedHttpActionDescriptor reflectedActionDescriptor = actionDescriptor as ReflectedHttpActionDescriptor;
            if (reflectedActionDescriptor != null)
            {
                string selectExpression = string.Format(_methodExpression, GetMemberName(reflectedActionDescriptor.MethodInfo));
                XPathNavigator node = _documentNavigator.SelectSingleNode(selectExpression);
                if (node != null)
                {
                    return node;
                }
            }

            return null;
        }

        private XPathNavigator GetMemberReturnsNode(HttpActionDescriptor actionDescriptor)
        {
            ReflectedHttpActionDescriptor reflectedActionDescriptor = actionDescriptor as ReflectedHttpActionDescriptor;
            if (reflectedActionDescriptor != null)
            {
                string selectExpression = string.Format(_returnsExpression, GetMemberName(reflectedActionDescriptor.MethodInfo));
                XPathNavigator node = _documentNavigator.SelectSingleNode(selectExpression);
                if (node != null)
                {
                    return node;
                }
            }
            return null;
        }

        private object GetMemberResponseCodeSummary(HttpActionDescriptor actionDescriptor)
        {
            var summary = string.Empty;
            foreach (var node in GetMemberResponseCodeNodes(actionDescriptor))
            {
                summary += string.Format("Code: {0}, Reason: {1}{2}"
                    , node.GetAttribute("code", string.Empty)
                    , node.ToString()
                    , _newLine);
            }
            return string.IsNullOrEmpty(summary) ? summary : string.Format("Response codes:{0}{1}", _newLine, summary);
        }

        private IEnumerable<XPathNavigator> GetMemberResponseCodeNodes(HttpActionDescriptor actionDescriptor)
        {
            ReflectedHttpActionDescriptor reflectedActionDescriptor = actionDescriptor as ReflectedHttpActionDescriptor;
            if (reflectedActionDescriptor != null)
            {
                string selectExpression = string.Format(_responseCodeExpression, GetMemberName(reflectedActionDescriptor.MethodInfo));
                var nodes = _documentNavigator.Select(selectExpression);
                while (nodes.MoveNext())
                {
                    yield return nodes.Current;
                }
            }
        }

        private static string GetMemberName(MethodInfo method)
        {
            string name = string.Format("{0}.{1}", method.DeclaringType.FullName, method.Name);
            var parameters = method.GetParameters();
            if (parameters.Length != 0)
            {
                string[] parameterTypeNames = parameters.Select(param => ProcessTypeName(param.ParameterType.FullName)).ToArray();
                name += string.Format("({0})", string.Join(",", parameterTypeNames));
            }

            return name;
        }

        private static string ProcessTypeName(string typeName)
        {
            //handle nullable
            var result = nullableTypeNameRegex.Match(typeName);
            if (result.Success)
            {
                return string.Format("{0}{{{1}}}", result.Groups[1].Value, result.Groups[2].Value);
            }
            return typeName;
        }

        internal string GetParamName(Type parameterType)
        {
            var result = nullableTypeNameRegex.Match(parameterType.FullName);
            if (result.Success)
            {
                var genericParameterType = parameterType.GetGenericArguments().FirstOrDefault();
                if (genericParameterType != null)
                    return GetMappedParamName(genericParameterType.Name);
            }
            return GetMappedParamName(parameterType.Name);
        }

        private string GetMappedParamName(string parameterTypeName)
        {
            string selectExpression = string.Format(_dataTypeExpression, parameterTypeName);
            XPathNavigator node = _dataTypeMapNavigator.SelectSingleNode(selectExpression);
            if (node != null)
            {
                var type = node.GetAttribute("JSValue", string.Empty);
                if (!string.IsNullOrEmpty(type))
                    return type;
            }
            return parameterTypeName;
        }

        public string DataTypeMapFilePath
        {
            set
            {
                XPathDocument xpath = new XPathDocument(value);
                _dataTypeMapNavigator = xpath.CreateNavigator();
            }
        }
    }
}

