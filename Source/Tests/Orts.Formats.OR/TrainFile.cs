﻿// COPYRIGHT 2020 by the Open Rails project.
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

using Newtonsoft.Json;
using Orts.Formats.OR;
using ORTS.Content;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Tests.Orts.Formats.OR
{
    public class TrainFileTests
    {
        private static readonly IDictionary<string, string> Folders = new Dictionary<string, string>();

        #region List train type
        [Fact]
        private static void GetListForwardWagonReferences()
        {
            var train = new ListTrainFile()
            {
                DisplayName = "Test train",
                PlayerDrivable = true,
                List = new ListTrainItem[]
                {
                    new ListTrainEngine()
                    {
                        Engine = "SomeLocomotiveA",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon",
                    },
                    new ListTrainEngine()
                    {
                        Engine = "SomeLocomotiveB",
                        Flip = true,
                    },
                },
            };
            using (var content = new TestContent())
            {
                var expected = new[]
                {
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeLocomotiveA.eng"), false, 0),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeWagon.wag"), false, 1),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeWagon.wag"), false, 2),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeLocomotiveB.eng"), true, 3),
                };
                Assert.Equal(expected, train.GetForwardWagonList(content.Path, Folders));
            }
        }

        [Fact]
        private static void GetListReverseWagonReferences()
        {
            var train = new ListTrainFile()
            {
                DisplayName = "Test train",
                PlayerDrivable = true,
                List = new ListTrainItem[]
                {
                    new ListTrainEngine()
                    {
                        Engine = "SomeLocomotiveA",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon",
                    },
                    new ListTrainEngine()
                    {
                        Engine = "SomeLocomotiveB",
                        Flip = true,
                    },
                },
            };
            using (var content = new TestContent())
            {
                var expected = new[]
                {
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeLocomotiveB.eng"), false, 0),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeWagon.wag"), true, 1),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeWagon.wag"), true, 2),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeLocomotiveA.eng"), true, 3),
                };
                Assert.Equal(expected, train.GetReverseWagonList(content.Path, Folders));
            }
        }

        [Fact]
        private static void GetListRepeatedForwardWagonReferences()
        {
            var train = new ListTrainFile()
            {
                DisplayName = "Test train",
                PlayerDrivable = true,
                List = new ListTrainItem[]
                {
                    new ListTrainEngine()
                    {
                        Engine = "SomeLocomotiveA",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon",
                        Count = 3,
                    },
                    new ListTrainEngine()
                    {
                        Engine = "SomeLocomotiveB",
                        Flip = true,
                    },
                },
            };
            using (var content = new TestContent())
            {
                var expected = new[]
                {
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeLocomotiveA.eng"), false, 0),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeWagon.wag"), false, 1),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeWagon.wag"), false, 2),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeWagon.wag"), false, 3),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeLocomotiveB.eng"), true, 4),
                };
                Assert.Equal(expected, train.GetForwardWagonList(content.Path, Folders));
            }
        }

        [Fact]
        private static void GetListRepeatedReverseWagonReferences()
        {
            var train = new ListTrainFile()
            {
                DisplayName = "Test train",
                PlayerDrivable = true,
                List = new ListTrainItem[]
                {
                    new ListTrainEngine()
                    {
                        Engine = "SomeLocomotiveA",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon",
                        Count = 3,
                    },
                    new ListTrainEngine()
                    {
                        Engine = "SomeLocomotiveB",
                        Flip = true,
                    },
                },
            };
            using (var content = new TestContent())
            {
                var expected = new[]
                {
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeLocomotiveB.eng"), false, 0),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeWagon.wag"), true, 1),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeWagon.wag"), true, 2),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeWagon.wag"), true, 3),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeLocomotiveA.eng"), true, 4),
                };
                Assert.Equal(expected, train.GetReverseWagonList(content.Path, Folders));
            }
        }

        [Fact]
        private static void GetListLeadLocomotiveChoices()
        {
            var train = new ListTrainFile()
            {
                DisplayName = "Test train",
                PlayerDrivable = true,
                List = new ListTrainItem[]
                {
                    new ListTrainEngine()
                    {
                        Engine = "SomeLocomotiveA",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon",
                    },
                    new ListTrainEngine()
                    {
                        Engine = "SomeLocomotiveB",
                        Flip = true,
                    },
                },
            };
            using (var content = new TestContent())
            {
                var expected = new HashSet<PreferredLocomotive>()
                {
                    new PreferredLocomotive(Path.Combine(content.TrainsetPath, "SomeLocomotiveA.eng")),
                };
                Assert.Equal(expected, train.GetLeadLocomotiveChoices(content.Path, Folders));
            }
        }

        [Fact]
        private static void GetListReverseLocomotiveChoices()
        {
            var train = new ListTrainFile()
            {
                DisplayName = "Test train",
                PlayerDrivable = true,
                List = new ListTrainItem[]
                {
                    new ListTrainEngine()
                    {
                        Engine = "SomeLocomotiveA",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon",
                    },
                    new ListTrainEngine()
                    {
                        Engine = "SomeLocomotiveB",
                        Flip = true,
                    },
                },
            };
            using (var content = new TestContent())
            {
                var expected = new HashSet<PreferredLocomotive>()
                {
                    new PreferredLocomotive(Path.Combine(content.TrainsetPath, "SomeLocomotiveB.eng")),
                };
                Assert.Equal(expected, train.GetReverseLocomotiveChoices(content.Path, Folders));
            }
        }

        [Fact]
        private static void GetListLeadLocomotiveChoicesWithoutEngine()
        {
            var train = new ListTrainFile()
            {
                DisplayName = "Test train",
                PlayerDrivable = false,
                List = new ListTrainItem[]
                {
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon",
                    },
                },
            };
            using (var content = new TestContent())
                Assert.Equal(PreferredLocomotive.NoLocomotiveSet, train.GetLeadLocomotiveChoices(content.Path, Folders));
        }

        [Fact]
        private static void GetListReverseLocomotiveChoicesWithoutEngine()
        {
            var train = new ListTrainFile()
            {
                DisplayName = "Test train",
                PlayerDrivable = false,
                List = new ListTrainItem[]
                {
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon",
                    },
                },
            };
            using (var content = new TestContent())
                Assert.Equal(PreferredLocomotive.NoLocomotiveSet, train.GetReverseLocomotiveChoices(content.Path, Folders));
        }

        [Fact]
        private static void GetEmptyListLeadLocomotiveChoices()
        {
            var train = new ListTrainFile()
            {
                DisplayName = "Test train",
                PlayerDrivable = false,
                List = new ListTrainItem[] { },
            };
            using (var content = new TestContent())
                Assert.Empty(train.GetLeadLocomotiveChoices(content.Path, Folders));
        }

        [Fact]
        private static void GetEmptyListReverseLocomotiveChoices()
        {
            var train = new ListTrainFile()
            {
                DisplayName = "Test train",
                PlayerDrivable = false,
                List = new ListTrainItem[] { },
            };
            using (var content = new TestContent())
                Assert.Empty(train.GetReverseLocomotiveChoices(content.Path, Folders));
        }

        [Fact]
        public static void GetListForwardWagonReferencesGivenUnsatisifablePreference()
        {
            var train = new ListTrainFile()
            {
                DisplayName = "Test train",
                PlayerDrivable = true,
                List = new ListTrainItem[]
                {
                    new ListTrainEngine()
                    {
                        Engine = "SomeLocomotive",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon",
                    },
                },
            };
            using (var content = new TestContent())
            {
                var unsatisfiable = new PreferredLocomotive(Path.Combine(content.TrainsetPath, "acela", "acela.eng"));
                Assert.Empty(train.GetForwardWagonList(content.Path, Folders, preference: unsatisfiable));
            }
        }

        [Fact]
        public static void GetListReverseWagonReferencesGivenUnsatisifablePreference()
        {
            var train = new ListTrainFile()
            {
                DisplayName = "Test train",
                PlayerDrivable = true,
                List = new ListTrainItem[]
                {
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon",
                    },
                    new ListTrainEngine()
                    {
                        Engine = "SomeLocomotive",
                        Flip = true,
                    },
                },
            };
            using (var content = new TestContent())
            {
                var unsatisfiable = new PreferredLocomotive(Path.Combine(content.TrainsetPath, "acela", "acela.eng"));
                Assert.Empty(train.GetForwardWagonList(content.Path, Folders, preference: unsatisfiable));
            }
        }
        #endregion

        #region List train type -> List train type
        [Fact]
        public static void GetListInListForwardWagonReferences()
        {
            var parentTrain = new ListTrainFile()
            {
                DisplayName = "Parent train",
                PlayerDrivable = true,
                List = new ListTrainItem[]
                {
                    new ListTrainEngine()
                    {
                        Engine = "SomeLocomotiveA",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon1",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon2",
                    },
                    new ListTrainReference()
                    {
                        Train = "child",
                    },
                },
            };
            var childTrain = new ListTrainFile()
            {
                DisplayName = "Child train",
                PlayerDrivable = true,
                List = new ListTrainItem[]
                {
                    new ListTrainEngine()
                    {
                        Engine = "SomeLocomotiveB",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon3",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon4",
                    },
                },
            };
            using (var content = new TestContent())
            {
                File.WriteAllText(Path.Combine(content.ConsistsPath, "parent.train-or"), JsonConvert.SerializeObject(parentTrain));
                File.WriteAllText(Path.Combine(content.ConsistsPath, "child.train-or"), JsonConvert.SerializeObject(childTrain));
                var expected = new[]
                {
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeLocomotiveA.eng"), false, 0),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeWagon1.wag"), false, 1),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeWagon2.wag"), false, 2),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeLocomotiveB.eng"), false, 3),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeWagon3.wag"), false, 4),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeWagon4.wag"), false, 5),
                };
                Assert.Equal(expected, parentTrain.GetForwardWagonList(content.Path, Folders));
            }
        }

        [Fact]
        public static void GetReverseListInListForwardWagonReferences()
        {
            var parentTrain = new ListTrainFile()
            {
                DisplayName = "Parent train",
                PlayerDrivable = true,
                List = new ListTrainItem[]
                {
                    new ListTrainEngine()
                    {
                        Engine = "SomeLocomotiveA",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon1",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon2",
                    },
                    new ListTrainReference()
                    {
                        Train = "child",
                        Flip = true,
                    },
                },
            };
            var childTrain = new ListTrainFile()
            {
                DisplayName = "Child train",
                PlayerDrivable = true,
                List = new ListTrainItem[]
                {
                    new ListTrainEngine()
                    {
                        Engine = "SomeLocomotiveB",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon3",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon4",
                    },
                },
            };
            using (var content = new TestContent())
            {
                File.WriteAllText(Path.Combine(content.ConsistsPath, "parent.train-or"), JsonConvert.SerializeObject(parentTrain));
                File.WriteAllText(Path.Combine(content.ConsistsPath, "child.train-or"), JsonConvert.SerializeObject(childTrain));
                var expected = new[]
                {
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeLocomotiveA.eng"), false, 0),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeWagon1.wag"), false, 1),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeWagon2.wag"), false, 2),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeWagon4.wag"), true, 3),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeWagon3.wag"), true, 4),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeLocomotiveB.eng"), true, 5),
                };
                Assert.Equal(expected, parentTrain.GetForwardWagonList(content.Path, Folders));
            }
        }

        [Fact]
        public static void GetReverseListInListReverseWagonReferences()
        {
            var parentTrain = new ListTrainFile()
            {
                DisplayName = "Parent train",
                PlayerDrivable = true,
                List = new ListTrainItem[]
                {
                    new ListTrainEngine()
                    {
                        Engine = "SomeLocomotiveA",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon1",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon2",
                    },
                    new ListTrainReference()
                    {
                        Train = "child",
                        Flip = true,
                    },
                },
            };
            var childTrain = new ListTrainFile()
            {
                DisplayName = "Child train",
                PlayerDrivable = true,
                List = new ListTrainItem[]
                {
                    new ListTrainEngine()
                    {
                        Engine = "SomeLocomotiveB",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon3",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon4",
                    },
                },
            };
            using (var content = new TestContent())
            {
                File.WriteAllText(Path.Combine(content.ConsistsPath, "parent.train-or"), JsonConvert.SerializeObject(parentTrain));
                File.WriteAllText(Path.Combine(content.ConsistsPath, "child.train-or"), JsonConvert.SerializeObject(childTrain));
                var expected = new[]
                {
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeLocomotiveB.eng"), false, 0),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeWagon3.wag"), false, 1),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeWagon4.wag"), false, 2),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeWagon2.wag"), true, 3),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeWagon1.wag"), true, 4),
                    new WagonReference(Path.Combine(content.TrainsetPath, "SomeLocomotiveA.eng"), true, 5),
                };
                Assert.Equal(expected, parentTrain.GetReverseWagonList(content.Path, Folders));
            }
        }

        [Fact]
        public static void GetListInListLeadLocomotiveChoices()
        {
            var parentTrain = new ListTrainFile()
            {
                DisplayName = "Parent train",
                PlayerDrivable = true,
                List = new ListTrainItem[]
                {
                    new ListTrainReference()
                    {
                        Train = "child",
                    },
                    new ListTrainEngine()
                    {
                        Engine = "SomeLocomotiveA",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon1",
                    },
                },
            };
            var childTrain = new ListTrainFile()
            {
                DisplayName = "Child train",
                PlayerDrivable = true,
                List = new ListTrainItem[]
                {
                    new ListTrainEngine()
                    {
                        Engine = "SomeLocomotiveB",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon2",
                    },
                },
            };
            using (var content = new TestContent())
            {
                File.WriteAllText(Path.Combine(content.ConsistsPath, "parent.train-or"), JsonConvert.SerializeObject(parentTrain));
                File.WriteAllText(Path.Combine(content.ConsistsPath, "child.train-or"), JsonConvert.SerializeObject(childTrain));
                var expected = new HashSet<PreferredLocomotive>()
                {
                    new PreferredLocomotive(Path.Combine(content.TrainsetPath, "SomeLocomotiveB.eng")),
                };
                Assert.Equal(expected, parentTrain.GetLeadLocomotiveChoices(content.Path, Folders));
            }
        }

        [Fact]
        public static void GetListInListReverseLocomotiveChoicesWithLeadingWagon()
        {
            var parentTrain = new ListTrainFile()
            {
                DisplayName = "Parent train",
                PlayerDrivable = true,
                List = new ListTrainItem[]
                {
                    new ListTrainReference()
                    {
                        Train = "child",
                    },
                    new ListTrainEngine()
                    {
                        Engine = "SomeLocomotiveA",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon1",
                    },
                },
            };
            var childTrain = new ListTrainFile()
            {
                DisplayName = "Child train",
                PlayerDrivable = true,
                List = new ListTrainItem[]
                {
                    new ListTrainEngine()
                    {
                        Engine = "SomeLocomotiveB",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon2",
                    },
                },
            };
            using (var content = new TestContent())
            {
                File.WriteAllText(Path.Combine(content.ConsistsPath, "parent.train-or"), JsonConvert.SerializeObject(parentTrain));
                File.WriteAllText(Path.Combine(content.ConsistsPath, "child.train-or"), JsonConvert.SerializeObject(childTrain));
                var expected = new HashSet<PreferredLocomotive>()
                {
                    new PreferredLocomotive(Path.Combine(content.TrainsetPath, "SomeLocomotiveA.eng")),
                };
                Assert.Equal(expected, parentTrain.GetReverseLocomotiveChoices(content.Path, Folders));
            }
        }

        [Fact]
        public static void GetListInListLeadLocomotiveChoicesWithoutEngine()
        {
            var parentTrain = new ListTrainFile()
            {
                DisplayName = "Parent train",
                PlayerDrivable = false,
                List = new ListTrainItem[]
                {
                    new ListTrainReference()
                    {
                        Train = "child",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon1",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon2",
                    },
                },
            };
            var childTrain = new ListTrainFile()
            {
                DisplayName = "Child train",
                PlayerDrivable = false,
                List = new ListTrainItem[]
                {
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon3",
                    },
                    new ListTrainWagon()
                    {
                        Wagon = "SomeWagon4",
                    },
                },
            };
            using (var content = new TestContent())
            {
                File.WriteAllText(Path.Combine(content.ConsistsPath, "parent.train-or"), JsonConvert.SerializeObject(parentTrain));
                File.WriteAllText(Path.Combine(content.ConsistsPath, "child.train-or"), JsonConvert.SerializeObject(childTrain));
                Assert.Equal(PreferredLocomotive.NoLocomotiveSet, parentTrain.GetLeadLocomotiveChoices(content.Path, Folders));
            }
        }
        #endregion
    }
}
