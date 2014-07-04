﻿// COPYRIGHT 2010, 2011, 2012, 2013, 2014 by the Open Rails project.
// 
// This file is part of Open Rails.
// 
// Open Rails is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Open Rails is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Open Rails.  If not, see <http://www.gnu.org/licenses/>.

using Microsoft.Xna.Framework;
using MSTS.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MSTS.Formats
{
    /// <summary>
    /// A LightState object encapsulates the data for each State in the States subblock.
    /// </summary>
    public class LightState
    {
        public float Duration;
        public uint Color;
        public Vector3 Position;
        public float Radius;
        public Vector3 Azimuth;
        public Vector3 Elevation;
        public bool Transition;
        public float Angle;

        public LightState(STFReader stf)
        {
            stf.MustMatch("(");
            stf.ParseBlock(new[] {
                new STFReader.TokenProcessor("duration", ()=>{ Duration = stf.ReadFloatBlock(STFReader.UNITS.None, null); }),
                new STFReader.TokenProcessor("lightcolour", ()=>{ Color = stf.ReadHexBlock(null); }),
                new STFReader.TokenProcessor("position", ()=>{ Position = stf.ReadVector3Block(STFReader.UNITS.None, Vector3.Zero); }),
                new STFReader.TokenProcessor("radius", ()=>{ Radius = stf.ReadFloatBlock(STFReader.UNITS.Distance, null); }),
                new STFReader.TokenProcessor("azimuth", ()=>{ Azimuth = stf.ReadVector3Block(STFReader.UNITS.None, Vector3.Zero); }),
                new STFReader.TokenProcessor("elevation", ()=>{ Elevation = stf.ReadVector3Block(STFReader.UNITS.None, Vector3.Zero); }),
                new STFReader.TokenProcessor("transition", ()=>{ Transition = 1 <= stf.ReadFloatBlock(STFReader.UNITS.None, 0); }),
                new STFReader.TokenProcessor("angle", ()=>{ Angle = stf.ReadFloatBlock(STFReader.UNITS.None, null); }),
            });
        }

        public LightState(LightState state, bool reverse)
        {
            Duration = state.Duration;
            Color = state.Color;
            Position = state.Position;
            Radius = state.Radius;
            Azimuth = state.Azimuth;
            Elevation = state.Elevation;
            Transition = state.Transition;
            Angle = state.Angle;

            if (reverse)
            {
                Azimuth.X += 180;
                Azimuth.X %= 360;
                Azimuth.Y += 180;
                Azimuth.Y %= 360;
                Azimuth.Z += 180;
                Azimuth.Z %= 360;
                Position.X *= -1;
                Position.Z *= -1;
            }
        }
    }

    #region Light enums
    public enum LightType
    {
        Glow,
        Cone,
    }

    public enum LightHeadlightCondition
    {
        Ignore,
        Off,
        Dim,
        Bright,
        DimBright, // MSTSBin
        OffDim, // MSTSBin
        OffBright, // MSTSBin
        // TODO: DimBright?, // MSTSBin labels this the same as DimBright. Not sure what it means.
    }

    public enum LightUnitCondition
    {
        Ignore,
        Middle,
        First,
        Last,
        LastRev, // MSTSBin
        FirstRev, // MSTSBin
    }

    public enum LightPenaltyCondition
    {
        Ignore,
        No,
        Yes,
    }

    public enum LightControlCondition
    {
        Ignore,
        AI,
        Player,
    }

    public enum LightServiceCondition
    {
        Ignore,
        No,
        Yes,
    }

    public enum LightTimeOfDayCondition
    {
        Ignore,
        Day,
        Night,
    }

    public enum LightWeatherCondition
    {
        Ignore,
        Clear,
        Rain,
        Snow,
    }

    public enum LightCouplingCondition
    {
        Ignore,
        Front,
        Rear,
        Both,
    }
    #endregion

    /// <summary>
    /// The Light class encapsulates the data for each Light object 
    /// in the Lights block of an ENG/WAG file. 
    /// </summary>
    public class Light
    {
        public int Index;
        public LightType Type;
        public LightHeadlightCondition Headlight;
        public LightUnitCondition Unit;
        public LightPenaltyCondition Penalty;
        public LightControlCondition Control;
        public LightServiceCondition Service;
        public LightTimeOfDayCondition TimeOfDay;
        public LightWeatherCondition Weather;
        public LightCouplingCondition Coupling;
        public bool Cycle;
        public float FadeIn;
        public float FadeOut;
        public List<LightState> States = new List<LightState>();

        public Light(int index, STFReader stf)
        {
            Index = index;
            stf.MustMatch("(");
            stf.ParseBlock(new[] {
                new STFReader.TokenProcessor("type", ()=>{ Type = (LightType)stf.ReadIntBlock(null); }),
                new STFReader.TokenProcessor("conditions", ()=>{ stf.MustMatch("("); stf.ParseBlock(new[] {
                    new STFReader.TokenProcessor("headlight", ()=>{ Headlight = (LightHeadlightCondition)stf.ReadIntBlock(null); }),
                    new STFReader.TokenProcessor("unit", ()=>{ Unit = (LightUnitCondition)stf.ReadIntBlock(null); }),
                    new STFReader.TokenProcessor("penalty", ()=>{ Penalty = (LightPenaltyCondition)stf.ReadIntBlock(null); }),
                    new STFReader.TokenProcessor("control", ()=>{ Control = (LightControlCondition)stf.ReadIntBlock(null); }),
                    new STFReader.TokenProcessor("service", ()=>{ Service = (LightServiceCondition)stf.ReadIntBlock(null); }),
                    new STFReader.TokenProcessor("timeofday", ()=>{ TimeOfDay = (LightTimeOfDayCondition)stf.ReadIntBlock(null); }),
                    new STFReader.TokenProcessor("weather", ()=>{ Weather = (LightWeatherCondition)stf.ReadIntBlock(null); }),
                    new STFReader.TokenProcessor("coupling", ()=>{ Coupling = (LightCouplingCondition)stf.ReadIntBlock(null); }),
                });}),
                new STFReader.TokenProcessor("cycle", ()=>{ Cycle = 0 != stf.ReadIntBlock(null); }),
                new STFReader.TokenProcessor("fadein", ()=>{ FadeIn = stf.ReadFloatBlock(STFReader.UNITS.None, null); }),
                new STFReader.TokenProcessor("fadeout", ()=>{ FadeOut = stf.ReadFloatBlock(STFReader.UNITS.None, null); }),
                new STFReader.TokenProcessor("states", ()=>{
                    stf.MustMatch("(");
                    var count = stf.ReadInt(null);
                    stf.ParseBlock(new[] {
                        new STFReader.TokenProcessor("state", ()=>{
                            if (States.Count >= count)
                                STFException.TraceWarning(stf, "Skipped extra State");
                            else
                                States.Add(new LightState(stf));
                        }),
                    });
                    if (States.Count < count)
                        STFException.TraceWarning(stf, (count - States.Count).ToString() + " missing State(s)");
                }),
            });
        }

        public Light(Light light, bool reverse)
        {
            Index = light.Index;
            Type = light.Type;
            Headlight = light.Headlight;
            Unit = light.Unit;
            Penalty = light.Penalty;
            Control = light.Control;
            Service = light.Service;
            TimeOfDay = light.TimeOfDay;
            Weather = light.Weather;
            Coupling = light.Coupling;
            Cycle = light.Cycle;
            FadeIn = light.FadeIn;
            FadeOut = light.FadeOut;
            foreach (var state in light.States)
                States.Add(new LightState(state, reverse));

            if (reverse)
            {
                if (Unit == LightUnitCondition.First)
                    Unit = LightUnitCondition.FirstRev;
                else if (Unit == LightUnitCondition.Last)
                    Unit = LightUnitCondition.LastRev;
            }
        }
    }

    /// <summary>
    /// A Lights object is created for any engine or wagon having a 
    /// Lights block in its ENG/WAG file. It contains a collection of
    /// Light objects.
    /// Called from within the MSTSWagon class.
    /// </summary>
    public class LightCollection
    {
        public List<Light> Lights = new List<Light>();

        public LightCollection(STFReader stf)
        {
            stf.MustMatch("(");
            stf.ReadInt(null); // count; ignore this because its not always correct
            stf.ParseBlock(new[] {
                new STFReader.TokenProcessor("light", ()=>{ Lights.Add(new Light(Lights.Count, stf)); }),
            });
            if (Lights.Count == 0)
                throw new InvalidDataException("lights with no lights");

            // MSTSBin created reverse headlight cones automatically, so we shall do so too.
            foreach (var light in Lights.ToArray())
                if (light.Type == LightType.Cone)
                    Lights.Add(new Light(light, true));
        }
    }
}