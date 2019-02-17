﻿//----------------------------------------------------------------------------
// Simple Control (Sc) - Version 1.0
// A high quality control rendering engine for C#
// Copyright (C) 2016-2017 cymheart
// Contact: 
//
// 
// Sc is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
// 
// Sc is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Sc; if not, write to the Free Software
//----------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sc
{
    public class ScGraphics : IScGraphics,IDisposable
    {
        public ScLayer layer;
        public virtual GraphicsType GetGraphicsType()
        {
            return GraphicsType.UNKNOWN;
        }
         
        public virtual void BeginDraw()
        {
            
        }

        public virtual void EndDraw()
        {
            
        }

        public virtual void ResetClip()
        {
           
        }

        public virtual void ResetTransform()
        {
            
        }

        public virtual void SetClip(RectangleF clipRect)
        {
            
        }

        public virtual void TranslateTransform(float dx, float dy)
        {
           
        }

        public virtual void ReSize(int width, int height)
        {

        }

        public virtual void Dispose()
        {
            
        }

        public virtual Matrix Transform
        {
            get { return null; }
            set { }
        }
    }
}
