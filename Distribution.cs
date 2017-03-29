/**
 * Copyright (C) 2016 Open University (http://www.ou.nl/)
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;

namespace StudentFeedback_SpaceModules
{
    class Distribution
    {
        public double mean;
        public double min;
        public double max;
        public double sum;
        public double skewness;
        public double kurtosis;
        public double variance;
        public double standardDeviation;
        public int n;
        public double helper1;
        public double helper2;
        public double helper3;
        public Boolean normal;
    }
}
