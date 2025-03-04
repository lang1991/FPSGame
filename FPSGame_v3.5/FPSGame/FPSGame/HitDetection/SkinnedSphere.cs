﻿#region File Description
//-----------------------------------------------------------------------------
// SkinnedSphere.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
#endregion

namespace FPSGame
{
    /// <summary>
    /// Stores a bounding sphere for checking collision against a skinned
    /// model, and records which bone this sphere is attached to.
    /// </summary>
    public class SkinnedSphere
    {
        public string BoneName;
        public float Radius;

        [ContentSerializer(Optional = true)]
        public float XOffset;

        [ContentSerializer(Optional = true)]
        public float YOffset;

        [ContentSerializer(Optional = true)]
        public float ZOffset;

        [ContentSerializer(Optional = true)]
        public float DamageValue;

        public bool highlighted;
    }
}
