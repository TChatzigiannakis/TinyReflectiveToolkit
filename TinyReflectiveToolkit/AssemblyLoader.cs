﻿/*
 *  Tiny Reflective Toolkit
    Copyright (C) 2014  Theodoros Chatzigiannakis

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>. 
 */


using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace TinyReflectiveToolkit
{
    /// <summary>
    /// A class that provides simple functionality for loading dependencies eagerly.
    /// </summary>
    public sealed class AssemblyLoader
    {
        /// <summary>
        /// This event is raised just before an assembly name is attempted to be loaded.
        /// </summary>
        public event Action<AssemblyName> AssemblyLoading;

        /// <summary>
        /// This event is raised just after an assembly has been loaded.
        /// </summary>
        public event Action<Assembly> AssemblyLoaded;

        /// <summary>
        /// This event is raised after a loading operation has been completed.
        /// </summary>
        public event Action<IEnumerable<Assembly>> AllAssembliesLoaded;

        /// <summary>
        /// This is the root assembly from which all other dependencies will be resolved.
        /// </summary>
        /// <value>The caller.</value>
        public Assembly Root { get; private set; }

        /// <summary>
        /// Creates a new AssemblyLoader, with the calling assembly as the root.
        /// </summary>
        public AssemblyLoader()
            : this(Assembly.GetCallingAssembly())
        {
        }

        /// <summary>
        /// Creates a new AssemblyLoader, with the provided assembly as the root.
        /// </summary>
        /// <param name="root">The root assembly.</param>
        public AssemblyLoader(Assembly root)
        {
            Root = root;
        }

        /// <summary>
        /// Load all the immediate dependencies of the root assembly.
        /// </summary>
        public void LoadImmediateDependencies()
        {
            var assemblies = Root.GetReferencedAssemblies().Select(Assembly.Load);

            if (AllAssembliesLoaded != null)
                AllAssembliesLoaded(assemblies);
        }

        /// <summary>
        /// Recursively load all dependencies, starting from the root assembly.
        /// </summary>
        public void LoadAllDependencies()
        {
            LoadAllDependencies(null as Func<AssemblyName, int>);
        }

        /// <summary>
        /// Recursively load all dependencies, starting from the root assembly, ordering them using the provided logic.
        /// </summary>
        public void LoadAllDependencies<T>(Func<AssemblyName, T> logic)
            where T : IComparable<T>
        {
            _loaded = new List<AssemblyName>();
            _assemblies = new List<Assembly>();

            _LoadAssembliesRecursively(Root.GetName(), logic);

            if (AllAssembliesLoaded != null)
                AllAssembliesLoaded(_assemblies);
        }

        private List<AssemblyName> _loaded = new List<AssemblyName>();
        private List<Assembly> _assemblies = new List<Assembly>();

        private bool _hasLoaded(AssemblyName name)
        {
            return _loaded.Any(x => x.ToString() == name.ToString());
        }

        private void _LoadAssembliesRecursively<T>(AssemblyName current, Func<AssemblyName, T> logic)
        {
            if (_hasLoaded(current))
                return;

            if (AssemblyLoading != null)
                AssemblyLoading(current);

            var asm = Assembly.Load(current);
            _loaded.Add(current);
            _assemblies.Add(asm);

            if (AssemblyLoaded != null)
                AssemblyLoaded(asm);

            var newReferencedQuery = asm.GetReferencedAssemblies()
                .Where(x => !_hasLoaded(x));
            if (logic != null)
                newReferencedQuery = newReferencedQuery.OrderBy(logic);
            var newReferenced = newReferencedQuery.ToList();

            foreach (var a in newReferenced)
                _LoadAssembliesRecursively(a, logic);
        }
    }
}
  