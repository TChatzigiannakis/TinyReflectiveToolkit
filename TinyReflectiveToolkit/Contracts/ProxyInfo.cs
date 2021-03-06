﻿/*
 * TinyReflectiveToolkit
 * Copyright (C) 2014-2015  Theodoros Chatzigiannakis
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EnumerableExtensions;

namespace TinyReflectiveToolkit.Contracts
{
    internal class ProxyInfo
    {
        public Type ProvidedType = null;
        public Type Contract = null;

        public List<MethodInfo> RequiredRegularMethods = null;
        public List<MethodInfo> RequiredStaticMethods = null;
        public List<MethodInfo> RequiredExplicitConversions = null;
        public List<MethodInfo> RequiredImplicitConversions = null;
        public List<MethodInfo> RequiredLeftSideAdditionOperators = null;
        public List<MethodInfo> RequiredRightSideAdditionOperators = null;
        public List<MethodInfo> RequiredLeftSideSubtractionOperators = null;
        public List<MethodInfo> RequiredRightSideSubtractionOperators = null;
        public List<MethodInfo> RequiredLeftSideMultiplicationOperators = null;
        public List<MethodInfo> RequiredRightSideMultiplicationOperators = null;
        public List<MethodInfo> RequiredLeftSideDivisionOperators = null;
        public List<MethodInfo> RequiredRightSideDivisionOperators = null;
        public List<MethodInfo> RequiredLeftSideModulusOperators = null;
        public List<MethodInfo> RequiredRightSideModulusOperators = null;
        public List<MethodInfo> RequiredLeftSideEqualityOperators = null;
        public List<MethodInfo> RequiredRightSideEqualityOperators = null;
        public List<MethodInfo> RequiredLeftSideInequalityOperators = null;
        public List<MethodInfo> RequiredRightSideInequalityOperators = null;
        public List<MethodInfo> RequiredLeftSideGreaterThanOperators = null;
        public List<MethodInfo> RequiredRightSideGreaterThanOperators = null;
        public List<MethodInfo> RequiredLeftSideLessThanOperators = null;
        public List<MethodInfo> RequiredRightSideLessThanOperators = null;
        public List<MethodInfo> RequiredLeftSideGreaterThanOrEqualOperators = null;
        public List<MethodInfo> RequiredRightSideGreaterThanOrEqualOperators = null;
        public List<MethodInfo> RequiredLeftSideLessThanOrEqualOperators = null;
        public List<MethodInfo> RequiredRightSideLessThanOrEqualOperators = null;
        public List<MethodInfo> RequiredLeftSideExclusiveOrOperators = null;
        public List<MethodInfo> RequiredRightSideExclusiveOrOperators = null;
        public List<MethodInfo> RequiredLeftSideBitwiseOrOperators = null;
        public List<MethodInfo> RequiredRightSideBitwiseOrOperators = null;
        public List<MethodInfo> RequiredLeftSideBitwiseAndOperators = null;
        public List<MethodInfo> RequiredRightSideBitwiseAndOperators = null;

        public List<MethodInfo> FoundRegularMethods = null;
        public List<MethodInfo> FoundStaticMethods = null;
        public List<Tuple<string, MethodInfo, int>> FoundExplicitConversions = null;
        public List<Tuple<string, MethodInfo, int>> FoundImplicitConversions = null;
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideAdditionOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundRightSideAdditionOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideSubtractionOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundRightSideSubtractionOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideMultiplicationOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundRightSideMultiplicationOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideDivisionOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundRightSideDivisionOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideModulusOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundRightSideModulusOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideEqualityOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundRightSideEqualityOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideInequalityOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundRightSideInequalityOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideGreaterThanOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundRightSideGreaterThanOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideLessThanOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundRightSideLessThanOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideGreaterThanOrEqualOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundRightSideGreaterThanOrEqualOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideLessThanOrEqualOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundRightSideLessThanOrEqualOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideExclusiveOrOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundRightSideExclusiveOrOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideBitwiseOrOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundRightSideBitwiseOrOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideBitwiseAndOperators = null;
        public List<Tuple<string, MethodInfo, int>> FoundRightSideBitwiseAndOperators = null;

        public bool IsValid
        {
            get
            {
                if (RequiredRegularMethods.Count != FoundRegularMethods.Count) return false;
                if (RequiredStaticMethods.Count != FoundStaticMethods.Count) return false;
                if (RequiredExplicitConversions.Count != FoundExplicitConversions.Count) return false;
                if (RequiredImplicitConversions.Count != FoundImplicitConversions.Count) return false;

                var reqFields =
                    typeof (ProxyInfo).GetFields()
                        .Where(x => x.Name.StartsWith("Required") && x.Name.EndsWith("Operators"))
                        .ToList();
                var foundFields =
                    typeof (ProxyInfo).GetFields()
                        .Where(x => x.Name.StartsWith("Found") && x.Name.EndsWith("Operators"))
                        .ToList();
                for (var i = 0; i < reqFields.Count; i++)
                {
                    var list1 = (List<MethodInfo>) reqFields[i].GetValue(this);
                    var list2 = (List<Tuple<string, MethodInfo, int>>) foundFields[i].GetValue(this);
                    if (list1.Count != list2.Count) return false;
                }

                if (FoundRegularMethods.Any(x => x == null)) return false;
                if (AllFoundOperators.Any(x => x.Item2 == null)) return false;

                return true;
            }
        }

        public ProxyInfo()
        {
            typeof(ProxyInfo).GetFields()
                .Where(x => x.FieldType == typeof(List<MethodInfo>))
                .ForEach(x => x.SetValue(this, new List<MethodInfo>()));
            typeof(ProxyInfo).GetFields()
                .Where(x => x.FieldType == typeof(List<Tuple<string, MethodInfo, int>>))
                .ForEach(x => x.SetValue(this, new List<Tuple<string, MethodInfo, int>>()));
        }

        public IEnumerable<MethodInfo> AllRequiredOperators
        {
            get
            {
                return typeof (ProxyInfo).GetFields()
                    .Where(x => x.FieldType == typeof (List<MethodInfo>))
                    .Select(x => (List<MethodInfo>) x.GetValue(this))
                    .Flatten();
            }
        }
        public IEnumerable<Tuple<string, MethodInfo, int>> AllFoundOperators
        {
            get
            {
                return typeof (ProxyInfo).GetFields()
                    .Where(x => x.FieldType == typeof (List<Tuple<string, MethodInfo, int>>))
                    .Select(x => (List<Tuple<string, MethodInfo, int>>) x.GetValue(this))
                    .Flatten();
            }
        }

        public void ResolveBinaryOperators<TAttribute>(MethodInfo[] methodInfoList)
            where TAttribute : ExposeBinaryOperatorAttribute
        {
            var attributeName = typeof (TAttribute).Name.Replace("Attribute", "");
            var leftSideField = typeof (ProxyInfo).GetFields().Single(x => x.Name == "RequiredLeftSide" + attributeName + "Operators");
            var rightSideField = typeof(ProxyInfo).GetFields().Single(x => x.Name == "RequiredRightSide" + attributeName + "Operators");
            leftSideField.SetValue(this, methodInfoList.WithAttribute<TAttribute>(x => x.OperatorSide == OpSide.ThisLeft).ToList());
            rightSideField.SetValue(this, methodInfoList.WithAttribute<TAttribute>(x => x.OperatorSide == OpSide.ThisRight).ToList());
        }

        private List<MethodInfo> GetReqList<TAttribute>(string side)
            where TAttribute : ExposeBinaryOperatorAttribute
        {
            var attributeName = typeof(TAttribute).Name.Replace("Attribute", "");
            var field = typeof(ProxyInfo).GetFields().Single(x => x.Name == "Required" + side + "Side" + attributeName + "Operators");
            return (List<MethodInfo>)field.GetValue(this);
        }
        public List<MethodInfo> GetReqLeft<TAttribute>()
            where TAttribute : ExposeBinaryOperatorAttribute
        {
            return GetReqList<TAttribute>("Left");
        }
        public List<MethodInfo> GetReqRight<TAttribute>()
            where TAttribute : ExposeBinaryOperatorAttribute
        {
            return GetReqList<TAttribute>("Right");
        }

        private List<Tuple<string, MethodInfo, int>> GetFoundList<TAttribute>(string side)
            where TAttribute : ExposeBinaryOperatorAttribute
        {
            var attributeName = typeof(TAttribute).Name.Replace("Attribute", "");
            var field =
                typeof(ProxyInfo).GetFields().Single(x => x.Name == "Found" + side + "Side" + attributeName + "Operators");
            return (List<Tuple<string, MethodInfo, int>>)field.GetValue(this);
        }
        public List<Tuple<string, MethodInfo, int>> GetFoundLeft<TAttribute>()
            where TAttribute : ExposeBinaryOperatorAttribute
        {
            return GetFoundList<TAttribute>("Left");
        }
        public List<Tuple<string, MethodInfo, int>> GetFoundRight<TAttribute>()
            where TAttribute : ExposeBinaryOperatorAttribute
        {
            return GetFoundList<TAttribute>("Right");
        }

        public IEnumerable<string> Issues { get { return _issues; } }
        private readonly List<string> _issues = new List<string>();
        public void AddIssue(string message)
        {
            _issues.Add(message);
        }
    }
}
