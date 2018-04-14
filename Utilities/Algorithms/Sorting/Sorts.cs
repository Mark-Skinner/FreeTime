using System;
using System.Collections.Generic;
using System.Reflection;

namespace Utilities.Algorithms.Sorting
{
    public abstract class Sorts
    {
        public enum SortOrder
        {
            Ascending = 0,
            Descending = 1
        }

        #region Public Methods

        #region Generic Sorts

        /// <summary>
        /// Sorts a list based on the provided <paramref name="SortOrder"/>
        /// and <paramref name="SortProperty"/>. This method calls
        /// <see cref="IntroSort{T}(ref List{T}, SortOrder, string)"/> to sort the list.
        /// </summary>
        /// <typeparam name="T">The type of object in the list.</typeparam>
        /// <param name="Data">The list to sort.</param>
        /// <param name="SortOrder">The order to sort the list.</param>
        /// <param name="SortProperty">The property to compare whern sorting the list.</param>
        public static void Sort<T>(ref List<T> Data, SortOrder SortOrder, string SortProperty)
        {
            // get the property to sort by
            //PropertyInfo Property = Reflection.Reflection.GetProperty<T>(SortProperty);

            // sort the data
            //Data.Sort((x, y) =>
            //    SortOrder == SortOrder.Ascending ?
            //        Compare(x, y, Property) :
            //        Compare(y, x, Property));
            IntroSort(ref Data, SortOrder, SortProperty);
        }

        /// <summary>
        /// Sorts a list based on the provided <paramref name="SortOrder"/>
        /// and <paramref name="SortProperty"/>. This method calls
        /// <see cref="IntroSort{T}(ref List{T}, SortOrder, string)"/> to sort the list.
        /// </summary>
        /// <typeparam name="T">The type of object in the list.</typeparam>
        /// <param name="Data">The list to sort.</param>
        /// <param name="StartIndex">The index that denotes the beginning of the section to be sorted.</param>
        /// <param name="EndIndex">The index that denotes the ending of the section to be sorted.</param>
        /// <param name="SortOrder">The order to sort the list.</param>
        /// <param name="SortProperty">The property to compare whern sorting the list.</param>
        public static void Sort<T>(ref List<T> Data, int StartIndex, int EndIndex, SortOrder SortOrder, string SortProperty)
        {
            IntroSort(ref Data, StartIndex, EndIndex, SortOrder, SortProperty);
        }
        
        /// <summary>
        /// Sorts a collection based on the provided <paramref name="SortOrder"/>
        /// and <paramref name="SortProperty"/>. This method calls
        /// <see cref="IntroSort{T}(ref List{T}, SortOrder, string)"/> to sort the list.
        /// </summary>
        /// <typeparam name="T">The type of object in the list.</typeparam>
        /// <param name="Data">The collection to sort.</param>
        /// <param name="SortOrder">The order to sort the list.</param>
        /// <param name="SortProperty">The property to compare whern sorting the list.</param>
        public static void Sort<T>(ref IEnumerable<T> Data, SortOrder SortOrder, string SortProperty)
        {
            // check if data if null
            if (Data == null)
                throw new ArgumentNullException("Data", "The provided data was null.");

            List<T> data = new List<T>(Data);
            IntroSort(ref data, SortOrder, SortProperty);
            Data = data;
        }

        /// <summary>
        /// Sorts a collection based on the provided <paramref name="SortOrder"/>
        /// and <paramref name="SortProperty"/>. This method calls
        /// <see cref="IntroSort{T}(ref List{T}, SortOrder, string)"/> to sort the list.
        /// </summary>
        /// <typeparam name="T">The type of object in the list.</typeparam>
        /// <param name="Data">The collection to sort.</param>
        /// <param name="StartIndex">The index that denotes the beginning of the section to be sorted.</param>
        /// <param name="EndIndex">The index that denotes the ending of the section to be sorted.</param>
        /// <param name="SortOrder">The order to sort the list.</param>
        /// <param name="SortProperty">The property to compare whern sorting the list.</param>
        public static void Sort<T>(ref IEnumerable<T> Data, int StartIndex, int EndIndex, SortOrder SortOrder, string SortProperty)
        {
            // check if data if null
            if (Data == null)
                throw new ArgumentNullException("Data", "The provided data was null.");

            List<T> data = new List<T>(Data);
            IntroSort(ref data, StartIndex, EndIndex, SortOrder, SortProperty);
            Data = data;
        }

        public static void Sort<T>(ref System.Data.DataTable Data, SortOrder SortOrder, string SortProperty)
        {
            Data.DefaultView.Sort = SortProperty + (SortOrder == SortOrder.Ascending ? " ASC" : " DESC");
            Data = Data.DefaultView.ToTable();
        }

        #endregion

        #region Algorithmic Sorts
        
        /// <summary>
        /// Performs an introspective sort (hybrid sorting
        /// algorithm) on the provided <paramref name="Data"/> and sorts it
        /// based on the provided <paramref name="SortProperty"/> in the provided
        /// <paramref name="SortOrder"/>.
        /// </summary>
        /// <remarks>
        /// This algorithm uses an insertion sort if the size of a partition
        /// is greater than 16. It uses heapsort if the number of
        /// partitions is greater than 2*Log(N) where N is the number of
        /// elements. Otherwise, it uses quicksort with a default central pivot.
        /// </remarks>
        /// <typeparam name="T">The type of the items in <paramref name="Data"/>.</typeparam>
        /// <param name="Data">The data to sort.</param>
        /// <param name="SortOrder">The order to sort the data.</param>
        /// <param name="SortProperty">The property of an item of type <typeparamref name="T"/> to sort by.</param>
        public static void IntroSort<T>(ref List<T> Data, SortOrder SortOrder, string SortProperty)
        {
            IntroSort(ref Data, 0, Data.Count - 1, SortOrder, SortProperty);
        }

        /// <summary>
        /// Performs an introspective sort (hybrid sorting
        /// algorithm) on the provided <paramref name="Data"/> and sorts it
        /// based on the provided <paramref name="SortProperty"/> in the provided
        /// <paramref name="SortOrder"/>.
        /// </summary>
        /// <remarks>
        /// This algorithm uses an insertion sort if the size of a partition
        /// is greater than 16. It uses heapsort if the number of
        /// partitions is greater than 2*Log(N) where N is the number of
        /// elements. Otherwise, it uses quicksort with a default central pivot.
        /// </remarks>
        /// <typeparam name="T">The type of the items in <paramref name="Data"/>.</typeparam>
        /// <param name="Data">The data to sort.</param>
        /// <param name="StartIndex">The index that denotes the beginning of the section to be sorted.</param>
        /// <param name="EndIndex">The index that denotes the ending of the section to be sorted.</param>
        /// <param name="SortOrder">The order to sort the data.</param>
        /// <param name="SortProperty">The property of an item of type <typeparamref name="T"/> to sort by.</param>
        public static void IntroSort<T>(ref List<T> Data, int StartIndex, int EndIndex, SortOrder SortOrder, string SortProperty)
        {
            IntroSort(ref Data, StartIndex, EndIndex, SortOrder, Reflection.Reflection.GetProperty<T>(SortProperty));
        }

        /// <summary>
        /// Performs an introspective sort (hybrid sorting
        /// algorithm) on the provided <paramref name="Data"/> and sorts it
        /// based on the provided <paramref name="SortProperty"/> in the provided
        /// <paramref name="SortOrder"/>.
        /// </summary>
        /// <remarks>
        /// This algorithm uses an insertion sort if the size of a partition
        /// is greater than 16. It uses heapsort if the number of
        /// partitions is greater than 2*Log(N) where N is the number of
        /// elements. Otherwise, it uses quicksort with a default central pivot.
        /// </remarks>
        /// <typeparam name="T">The type of the items in <paramref name="Data"/>.</typeparam>
        /// <param name="Data">The data to sort.</param>
        /// <param name="StartIndex">The index that denotes the beginning of the section to be sorted.</param>
        /// <param name="EndIndex">The index that denotes the ending of the section to be sorted.</param>
        /// <param name="SortOrder">The order to sort the data.</param>
        /// <param name="Property">The property of an item of type <typeparamref name="T"/> to sort by.</param>
        public static void IntroSort<T>(ref List<T> Data, int StartIndex, int EndIndex, SortOrder SortOrder, PropertyInfo Property)
        {
            // check if data if null
            if (Data == null)
                throw new ArgumentNullException("Data", "The provided list was null.");

            // check if StartIndex or EndIndex are out of range
            if (StartIndex < 0)
                throw new ArgumentOutOfRangeException("StartIndex", StartIndex, "The start index was less than zero.");

            if (EndIndex >= Data.Count)
                throw new ArgumentOutOfRangeException("EndIndex", EndIndex, "The end index was greater than or equal to the size of the provided list of data.");

            // check if sort property is null
            if (Property == null)
                throw new ArgumentNullException("Property", "The sort property was null.");

            int left = StartIndex, // start index of current partition
                right = EndIndex, // end index of current partition
                index = 0, // used to get current partition bounds
                pivot_index = 0; // pivot returned from Partition method

            const int MIN_PARTITION_SIZE = 16;
            double MAX_NUM_OF_PARTITIONS = 4 * Math.Log(EndIndex - StartIndex + 1, 2); // partition limit, NOTE: It's 4 times because 2 indices = 1 partition

            // array to hold partitions (indices for beginning and end index of each partition)
            int[] indices = new int[EndIndex - StartIndex + 1];

            // start with the part of the array bounded by left and right
            indices[index++] = left;
            indices[index] = right;

            // while there are partitions..
            while (index >= 0)
            {
                right = indices[index--];
                left = indices[index--];

                // if the partition size is very small.. use insertion sort
                if (right - left + 1 <= MIN_PARTITION_SIZE)
                {
                    // insertion sort
                    _InsertionSort(ref Data, left, right, SortOrder, Property);
                }
                // else if there are a lot of partitions.. use heap sort
                else if (index + 2 >= MAX_NUM_OF_PARTITIONS)
                {
                    // heap sort
                    _HeapSort(ref Data, left, right, SortOrder, Property);
                }
                // otherwise use modified quicksort
                else
                {
                    // partition
                    pivot_index = Partition(ref Data, left, right, SortOrder, Property);

                    // if the pivot is not all the way to the left..
                    if (pivot_index - 1 > left)
                    {
                        indices[++index] = left;
                        indices[++index] = pivot_index - 1;
                    }

                    // if the pivot is not all the way to the right..
                    if (pivot_index + 1 < right)
                    {
                        indices[++index] = pivot_index + 1;
                        indices[++index] = right;
                    }
                }
            }
        }

        public static void QuickSort<T>(ref List<T> Data, SortOrder SortOrder, string SortProperty)
        {
            QuickSort(ref Data, 0, Data.Count - 1, SortOrder, SortProperty);
        }

        public static void QuickSort<T>(ref List<T> Data, int StartIndex, int EndIndex, SortOrder SortOrder, string SortProperty)
        {
            QuickSort(ref Data, StartIndex, EndIndex, SortOrder, Reflection.Reflection.GetProperty<T>(SortProperty));
        }

        public static void QuickSort<T>(ref List<T> Data, int StartIndex, int EndIndex, SortOrder SortOrder, PropertyInfo Property)
        {
            // check if data if null
            if (Data == null)
                throw new ArgumentNullException("Data", "The provided list was null.");

            // check if StartIndex or EndIndex are out of range
            if (StartIndex < 0)
                throw new ArgumentOutOfRangeException("StartIndex", StartIndex, "The start index was less than zero.");

            if (EndIndex >= Data.Count)
                throw new ArgumentOutOfRangeException("EndIndex", EndIndex, "The end index was greater than or equal to the size of the provided list of data.");

            // check if sort property is null
            if (Property == null)
                throw new ArgumentNullException("Property", "The sort property was null.");

            int left = StartIndex, // start index of current partition
                right = EndIndex, // end index of current partition
                index = 0, // used to get current partition bounds
                pivot_index = 0; // pivot returned from Partition method

            // array to hold partitions (indices for beginning and end index of each partition)
            int[] indices = new int[EndIndex - StartIndex + 1];

            // start with the entire array (0-count-1)
            indices[index++] = left;
            indices[index] = right;
            
            // while there are partitions..
            while (index >= 0)
            {
                right = indices[index--];
                left = indices[index--];

                // partition
                pivot_index = Partition(ref Data, left, right, SortOrder, Property);

                // if the pivot is not all the way to the left..
                if (pivot_index - 1 > left)
                {
                    indices[++index] = left;
                    indices[++index] = pivot_index - 1;
                }

                // if the pivot is not all the way to the right..
                if (pivot_index + 1 < right)
                {
                    indices[++index] = pivot_index + 1;
                    indices[++index] = right;
                }
            }
        }
        
        public static void InsertionSort<T>(ref List<T> Data, SortOrder SortOrder, string SortProperty)
        {
            InsertionSort(ref Data, 0, Data.Count - 1, SortOrder, SortProperty);
        }

        public static void InsertionSort<T>(ref List<T> Data, int StartIndex, int EndIndex, SortOrder SortOrder, string SortProperty)
        {
            InsertionSort(ref Data, StartIndex, EndIndex, SortOrder, Reflection.Reflection.GetProperty<T>(SortProperty));
        }

        public static void InsertionSort<T>(ref List<T> Data, int StartIndex, int EndIndex, SortOrder SortOrder, PropertyInfo Property)
        {
            // check if data if null
            if (Data == null)
                throw new ArgumentNullException("Data", "The provided list was null.");

            // check if StartIndex or EndIndex are out of range
            if (StartIndex < 0)
                throw new ArgumentOutOfRangeException("StartIndex", StartIndex, "The start index was less than zero.");

            if (EndIndex >= Data.Count)
                throw new ArgumentOutOfRangeException("EndIndex", EndIndex, "The end index was greater than or equal to the size of the provided list of data.");

            // check if sort property is null
            if (Property == null)
                throw new ArgumentNullException("Property", "The sort property was null.");

            int j;
            T value;
            for (int i = StartIndex + 1; i <= EndIndex; i++)
            {
                value = Data[i];
                for (j = i-1; j >= StartIndex && Compare(value, Data[j], SortOrder, Property) < 0; j--)
                    Data[j + 1] = Data[j];
                if (j + 1 >= StartIndex)
                    Data[j + 1] = value;
            }
        }
        
        public static void HeapSort<T>(ref List<T> Data, SortOrder SortOrder, string SortProperty)
        {
            HeapSort(ref Data, 0, Data.Count - 1, SortOrder, SortProperty);
        }

        public static void HeapSort<T>(ref List<T> Data, int StartIndex, int EndIndex, SortOrder SortOrder, string SortProperty)
        {
            HeapSort(ref Data, StartIndex, EndIndex, SortOrder, Reflection.Reflection.GetProperty<T>(SortProperty));
        }

        public static void HeapSort<T>(ref List<T> Data, int StartIndex, int EndIndex, SortOrder SortOrder, PropertyInfo Property)
        {
            // check if data if null
            if (Data == null)
                throw new ArgumentNullException("Data", "The provided list was null.");

            // check if StartIndex or EndIndex are out of range
            if (StartIndex < 0)
                throw new ArgumentOutOfRangeException("StartIndex", StartIndex, "The start index was less than zero.");

            if (EndIndex >= Data.Count)
                throw new ArgumentOutOfRangeException("EndIndex", EndIndex, "The end index was greater than or equal to the size of the provided list of data.");

            // check if sort property is null
            if (Property == null)
                throw new ArgumentNullException("Property", "The sort property was null.");

            Heapify(ref Data, StartIndex, EndIndex, SortOrder, Property);

            for (int i = EndIndex; i > StartIndex; i--)
            {
                Swap(ref Data, i, StartIndex);
                SiftDown(ref Data, StartIndex, StartIndex, i - 1, SortOrder, Property);
            }
        }

        #endregion

        #endregion

        #region Private Methods

        #region Comparison

        /// <summary>
        /// Compares two objects based on the provided
        /// property using a default comparer.
        /// </summary>
        /// <typeparam name="T">The type of the provided objects.</typeparam>
        /// <param name="Object1">The first object to compare.</param>
        /// <param name="Object2">The second objec to compare.</param>
        /// <param name="Property">The property the objects will be compared by.</param>
        /// <returns></returns>
        private static int Compare<T>(T Object1, T Object2, PropertyInfo Property)
        {
            // get the values of the properties for each object
            object val1, val2;
            if (!Reflection.Reflection.TryGetPropertyValue(Object1, Property, out val1) || !Reflection.Reflection.TryGetPropertyValue(Object2, Property, out val2))
                return 0;
            
            // compare the two properties
            return Comparer<object>.Default.Compare(val1, val2);
        }

        /// <summary>
        /// Compares two objects based on the provided
        /// property using a default comparer.
        /// </summary>
        /// <typeparam name="T">The type of the provided objects.</typeparam>
        /// <param name="Object1">The first object to compare.</param>
        /// <param name="Object2">The second objec to compare.</param>
        /// <param name="SortOrder">The order used to compare the two items.</param>
        /// <param name="Property">The property the objects will be compared by.</param>
        /// <returns></returns>
        private static int Compare<T>(T Object1, T Object2, SortOrder SortOrder, PropertyInfo Property)
        {
            return SortOrder == SortOrder.Ascending ?
                Compare(Object1, Object2, Property) :
                Compare(Object2, Object1, Property);
        }

        /// <summary>
        /// Swaps two item values.
        /// </summary>
        /// <typeparam name="T">The type of the provided objects.</typeparam>
        /// <param name="Item1">The first item to swap.</param>
        /// <param name="Item2">The second item to swap.</param>
        private static void Swap<T>(ref T Item1, ref T Item2)
        {
            T temp = Item1;
            Item1 = Item2;
            Item2 = temp;
        }

        /// <summary>
        /// Swaps two item values from the provided
        /// <paramref name="Data"/>.
        /// </summary>
        /// <typeparam name="T">The type of the provided objects.</typeparam>
        /// <param name="Data">The data that contains the items to swap.</param>
        /// <param name="Index1">The index of the first item to swap.</param>
        /// <param name="Index2">The index of the second item to swap.</param>
        private static void Swap<T>(ref List<T> Data, int Index1, int Index2)
        {
            T temp = Data[Index1];
            Data[Index1] = Data[Index2];
            Data[Index2] = temp;
        }

        #endregion

        #region Sorting

        /// <summary>
        /// Method used during quicksort to find the next pivot index. This
        /// method partitions the data into two sets, one set containing values
        /// less than the pivot value and the other set containing values greater
        /// than the pivot value.
        /// </summary>
        /// <remarks>
        /// This method uses a central pivot and attempts to move the pivot as
        /// little as possible by swapping values before the pivot with values
        /// after the pivot. By doing this, the pivot will only be moved when
        /// the left or right side of the pivot is filled with data. This also
        /// reduces the time taken to sort a pre-sorted set of data.
        /// </remarks>
        /// <typeparam name="T">The type of the provided objects.</typeparam>
        /// <param name="Data">The data to partition.</param>
        /// <param name="Left">The index to start at in the set of data.</param>
        /// <param name="Right">The index to end at in the set of data.</param>
        /// <param name="SortOrder">The order to sort the data.</param>
        /// <param name="Property">The property of an item of type <typeparamref name="T"/> to sort by.</param>
        /// <returns>The index of the pivot.</returns>
        private static int Partition<T>(ref List<T> Data, int Left, int Right, SortOrder SortOrder, PropertyInfo Property)
        {
            // quicksort with pivot in center of range
            int pivot_index = Left + (Right - Left) / 2,
                left_swap = Left - 1,
                right_swap = Right + 1;

            T pivot = Data[pivot_index];

            while (true)
            {
                do
                    left_swap++;
                while (left_swap < pivot_index && Compare(Data[left_swap], pivot, SortOrder, Property) < 0);
                do
                    right_swap--;
                while (right_swap > pivot_index && Compare(Data[right_swap], pivot, SortOrder, Property) > 0);

                if (left_swap == pivot_index)
                {
                    if (right_swap == pivot_index)
                        return pivot_index;
                    if (right_swap - pivot_index > 1)
                    {
                        // swap values and move pivot
                        T temp = Data[pivot_index + 1];
                        Data[pivot_index + 1] = pivot;
                        Data[pivot_index++] = Data[right_swap];
                        Data[right_swap++] = temp;
                    }
                    else
                    {
                        Swap(ref Data, pivot_index, right_swap);
                        return pivot_index + 1;
                    }
                }
                else if (right_swap == pivot_index)
                {
                    if (pivot_index - left_swap > 1)
                    {
                        // swap values and move pivot
                        T temp = Data[pivot_index - 1];
                        Data[pivot_index - 1] = pivot;
                        Data[pivot_index--] = Data[left_swap];
                        Data[left_swap--] = temp;
                    }
                    else
                    {
                        Swap(ref Data, pivot_index, left_swap);
                        return pivot_index - 1;
                    }
                }
                else
                    Swap(ref Data, left_swap, right_swap);
            }
        }

        private static void Heapify<T>(ref List<T> Data, int StartIndex, int EndIndex, SortOrder SortOrder, PropertyInfo Property)
        {
            // put data into heap order
            for (int i = StartIndex + (EndIndex - StartIndex + 1) / 2; i >= StartIndex; i--)
                SiftDown(ref Data, StartIndex, i, EndIndex, SortOrder, Property);
        }

        private static void SiftDown<T>(ref List<T> Data, int ActualStart, int StartIndex, int EndIndex, SortOrder SortOrder, PropertyInfo Property)
        {
            int root = StartIndex,
                left_child = 0,
                swap = 0;

            // while the root has children..
            while ((left_child = 2 * (root - ActualStart) + 1 + ActualStart) <= EndIndex)
            {
                // find largest value and set swap equal to that index

                // if root value < child value, swap = child, else swap = root
                // NOTE: depends on the sort order
                swap = Compare(Data[root], Data[left_child], SortOrder, Property) < 0 ?
                    left_child :
                    root;

                // compare current value being swapped with right child and update swap accordingly
                if (left_child + 1 <= EndIndex && Compare(Data[swap], Data[left_child + 1], SortOrder, Property) < 0)
                    swap = left_child + 1;

                // assuming that the children are correct, when the root is the largest value we are done
                if (swap == root)
                    return;

                // otherwise, swap the root and the larger child and keep going
                Swap(ref Data, root, swap);
                root = swap;
            }
        }

        private static void _QuickSort<T>(ref List<T> Data, int StartIndex, int EndIndex, SortOrder SortOrder, PropertyInfo Property)
        {
            int left = StartIndex, // start index of current partition
                right = EndIndex, // end index of current partition
                index = 0, // used to get current partition bounds
                pivot_index = 0; // pivot returned from Partition method

            // array to hold partitions (indices for beginning and end index of each partition)
            int[] indices = new int[EndIndex - StartIndex + 1];

            // start with the entire array (0-count-1)
            indices[index++] = left;
            indices[index] = right;

            // while there are partitions..
            while (index >= 0)
            {
                right = indices[index--];
                left = indices[index--];

                // partition
                pivot_index = Partition(ref Data, left, right, SortOrder, Property);

                // if the pivot is not all the way to the left..
                if (pivot_index - 1 > left)
                {
                    indices[++index] = left;
                    indices[++index] = pivot_index - 1;
                }

                // if the pivot is not all the way to the right..
                if (pivot_index + 1 < right)
                {
                    indices[++index] = pivot_index + 1;
                    indices[++index] = right;
                }
            }
        }

        private static void _InsertionSort<T>(ref List<T> Data, int StartIndex, int EndIndex, SortOrder SortOrder, PropertyInfo Property)
        {
            int j;
            T value;
            for (int i = StartIndex + 1; i <= EndIndex; i++)
            {
                value = Data[i];
                for (j = i - 1; j >= StartIndex && Compare(value, Data[j], SortOrder, Property) < 0; j--)
                    Data[j + 1] = Data[j];
                if (j + 1 >= StartIndex)
                    Data[j + 1] = value;
            }
        }

        private static void _HeapSort<T>(ref List<T> Data, int StartIndex, int EndIndex, SortOrder SortOrder, PropertyInfo Property)
        {
            Heapify(ref Data, StartIndex, EndIndex, SortOrder, Property);

            for (int i = EndIndex; i > StartIndex; i--)
            {
                Swap(ref Data, i, StartIndex);
                SiftDown(ref Data, StartIndex, StartIndex, i - 1, SortOrder, Property);
            }
        }
        
        #endregion

        #region Testing

        private class Test
        {
            public int id { get; set; }

            public override string ToString()
            {
                return "" + id;
            }
        }

        private static void TimeSorts()
        {
            System.Diagnostics.Debug.WriteLine("Testing sorts..");
            Random r = new Random();

            bool do_built_in = true,
                 do_introsort = true,
                 do_insertion_sort = false,
                 do_heapsort = false,
                 do_quicksort = false;

            int items = 500000;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            System.Diagnostics.Debug.WriteLine("Creating data..");
            List<Test> dummy_data,
                       dummy_data2 = new List<Test>(items),
                       sorted_asc = new List<Test>(items),
                       sorted_desc = new List<Test>(items),
                       all_same_item = new List<Test>(items);
            for (int i = 0; i < items; i++)
            {
                all_same_item.Add(new Test { id = 1 });
                dummy_data2.Add(new Test { id = r.Next(1, items*2) });
            }

            System.Diagnostics.Debug.WriteLine("Jumbling up the data..");
            for (int i = 0; i < dummy_data2.Count; i++)
                Swap(ref dummy_data2, i, r.Next(0, dummy_data2.Count - 1));

            #region Built-in Sort

            if (do_built_in)
            {
                System.Diagnostics.Debug.WriteLine("Starting built-in sort..");
                dummy_data = new List<Test>(dummy_data2);
                PropertyInfo Property = Reflection.Reflection.GetProperty<Test>("id");
                sw.Restart();
                dummy_data.Sort((x, y) => Compare(x, y, Property));
                sw.Stop();
                sorted_asc = new List<Test>(dummy_data);
                System.Diagnostics.Debug.WriteLine("Built-in sort ASC: " + sw.ElapsedMilliseconds + "ms");
                
                sw.Restart();
                dummy_data.Sort((x, y) => Compare(x, y, Property));
                sw.Stop();
                System.Diagnostics.Debug.WriteLine("Built-in sort (pre-sorted, ASC->ASC): " + sw.ElapsedMilliseconds + "ms");

                sw.Restart();
                dummy_data.Sort((x, y) => Compare(y, x, Property));
                sw.Stop();
                System.Diagnostics.Debug.WriteLine("Built-in sort (pre-sorted, ASC->DESC): " + sw.ElapsedMilliseconds + "ms");

                dummy_data = new List<Test>(all_same_item);
                sw.Restart();
                dummy_data.Sort((x, y) => Compare(x, y, Property));
                sw.Stop();
                System.Diagnostics.Debug.WriteLine("Built-in sort ASC (All same): " + sw.ElapsedMilliseconds + "ms");

                dummy_data = new List<Test>(dummy_data2);
                sw.Restart();
                dummy_data.Sort((x, y) => Compare(y, x, Property));
                sw.Stop();
                sorted_desc = new List<Test>(dummy_data);
                System.Diagnostics.Debug.WriteLine("Built-in sort DESC: " + sw.ElapsedMilliseconds + "ms");
            }
            
            #endregion

            #region Introsort

            if (do_introsort)
            {
                System.Diagnostics.Debug.WriteLine("Starting introsort sort..");
                dummy_data = new List<Test>(dummy_data2);
                sw.Restart();
                IntroSort(ref dummy_data, SortOrder.Ascending, "id");
                sw.Stop();
                System.Diagnostics.Debug.WriteLine("Introsort ASC: " + sw.ElapsedMilliseconds + "ms");

                // verify it is sorted
                for (int i = 0; i < dummy_data.Count; i++)
                {
                    if (dummy_data[i].id != sorted_asc[i].id)
                    {
                        System.Diagnostics.Debug.WriteLine("NOT SORTED");
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        foreach (Test t in dummy_data2)
                            sb.Append(t.id + "\t");
                        System.Diagnostics.Debug.WriteLine("ORIGINAL DATA:\t" + sb.ToString());

                        sb = new System.Text.StringBuilder();
                        foreach (Test t in dummy_data)
                            sb.Append(t.id + "\t");
                        System.Diagnostics.Debug.WriteLine("ATTEMPTED SORTED DATA:\t" + sb.ToString());

                        sb = new System.Text.StringBuilder();
                        foreach (Test t in sorted_asc)
                            sb.Append(t.id + "\t");
                        System.Diagnostics.Debug.WriteLine("SORTED DATA:\t" + sb.ToString());
                        break;
                    }
                }

                sw.Restart();
                IntroSort(ref dummy_data, SortOrder.Ascending, "id");
                sw.Stop();
                System.Diagnostics.Debug.WriteLine("Introsort (pre-sorted, ASC->ASC): " + sw.ElapsedMilliseconds + "ms");

                // verify it is sorted
                for (int i = 0; i < dummy_data.Count; i++)
                {
                    if (dummy_data[i].id != sorted_asc[i].id)
                    {
                        System.Diagnostics.Debug.WriteLine("NOT SORTED");
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        foreach (Test t in dummy_data)
                            sb.Append(t.id + "\t");
                        System.Diagnostics.Debug.WriteLine("DATA:\t" + sb.ToString());
                        break;
                    }
                }

                sw.Restart();
                IntroSort(ref dummy_data, SortOrder.Descending, "id");
                sw.Stop();
                System.Diagnostics.Debug.WriteLine("Introsort (pre-sorted, ASC->DESC): " + sw.ElapsedMilliseconds + "ms");

                // verify it is sorted
                for (int i = 0; i < dummy_data.Count; i++)
                {
                    if (dummy_data[i].id != sorted_desc[i].id)
                    {
                        System.Diagnostics.Debug.WriteLine("NOT SORTED");
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        foreach (Test t in dummy_data)
                            sb.Append(t.id + "\t");
                        System.Diagnostics.Debug.WriteLine("DATA:\t" + sb.ToString());
                        break;
                    }
                }

                dummy_data = new List<Test>(all_same_item);
                sw.Restart();
                IntroSort(ref dummy_data, SortOrder.Ascending, "id");
                sw.Stop();
                System.Diagnostics.Debug.WriteLine("Introsort ASC (All same): " + sw.ElapsedMilliseconds + "ms");

                dummy_data = new List<Test>(dummy_data2);
                sw.Restart();
                IntroSort(ref dummy_data, SortOrder.Descending, "id");
                sw.Stop();
                System.Diagnostics.Debug.WriteLine("Introsort DESC: " + sw.ElapsedMilliseconds + "ms");

                // verify it is sorted
                for (int i = 0; i < dummy_data.Count; i++)
                {
                    if (dummy_data[i].id != sorted_desc[i].id)
                    {
                        System.Diagnostics.Debug.WriteLine("NOT SORTED");
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        foreach (Test t in dummy_data)
                            sb.Append(t.id + "\t");
                        System.Diagnostics.Debug.WriteLine("DATA:\t" + sb.ToString());
                        break;
                    }
                }
            }
            
            #endregion
            
            #region Insertion Sort

            if (do_insertion_sort)
            {
                System.Diagnostics.Debug.WriteLine("Starting insertion sort..");
                dummy_data = new List<Test>(dummy_data2);
                sw.Restart();
                InsertionSort(ref dummy_data, SortOrder.Ascending, "id");
                sw.Stop();
                System.Diagnostics.Debug.WriteLine("Insertion ASC: " + sw.ElapsedMilliseconds + "ms");

                // verify it is sorted
                for (int i = 0; i < dummy_data.Count; i++)
                {
                    if (dummy_data[i].id != sorted_asc[i].id)
                    {
                        System.Diagnostics.Debug.WriteLine("NOT SORTED");
                        break;
                    }
                }

                sw.Restart();
                InsertionSort(ref dummy_data, SortOrder.Ascending, "id");
                sw.Stop();
                System.Diagnostics.Debug.WriteLine("Insertion (pre-sorted, ASC->ASC): " + sw.ElapsedMilliseconds + "ms");

                // verify it is sorted
                for (int i = 0; i < dummy_data.Count; i++)
                {
                    if (dummy_data[i].id != sorted_asc[i].id)
                    {
                        System.Diagnostics.Debug.WriteLine("NOT SORTED");
                        break;
                    }
                }

                sw.Restart();
                InsertionSort(ref dummy_data, SortOrder.Descending, "id");
                sw.Stop();
                System.Diagnostics.Debug.WriteLine("Insertion (pre-sorted, ASC->DESC): " + sw.ElapsedMilliseconds + "ms");

                // verify it is sorted
                for (int i = 0; i < dummy_data.Count; i++)
                {
                    if (dummy_data[i].id != sorted_desc[i].id)
                    {
                        System.Diagnostics.Debug.WriteLine("NOT SORTED");
                        break;
                    }
                }

                dummy_data = new List<Test>(all_same_item);
                sw.Restart();
                InsertionSort(ref dummy_data, SortOrder.Ascending, "id");
                sw.Stop();
                System.Diagnostics.Debug.WriteLine("Insertion ASC (All same): " + sw.ElapsedMilliseconds + "ms");

                dummy_data = new List<Test>(dummy_data2);
                sw.Restart();
                InsertionSort(ref dummy_data, SortOrder.Descending, "id");
                sw.Stop();
                System.Diagnostics.Debug.WriteLine("Insertion DESC: " + sw.ElapsedMilliseconds + "ms");

                // verify it is sorted
                for (int i = 0; i < dummy_data.Count; i++)
                {
                    if (dummy_data[i].id != sorted_desc[i].id)
                    {
                        System.Diagnostics.Debug.WriteLine("NOT SORTED");
                        break;
                    }
                }
            }
            
            #endregion
            
            #region Heapsort

            if (do_heapsort)
            {
                System.Diagnostics.Debug.WriteLine("Starting heapsort..");
                dummy_data = new List<Test>(dummy_data2);
                sw.Restart();
                HeapSort(ref dummy_data, SortOrder.Ascending, "id");
                sw.Stop();
                System.Diagnostics.Debug.WriteLine("Heapsort ASC: " + sw.ElapsedMilliseconds + "ms");

                // verify it is sorted
                for (int i = 0; i < dummy_data.Count; i++)
                {
                    if (dummy_data[i].id != sorted_asc[i].id)
                    {
                        System.Diagnostics.Debug.WriteLine("NOT SORTED");
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        foreach (Test t in dummy_data2)
                            sb.Append(t.id + "\t");
                        System.Diagnostics.Debug.WriteLine("ORIGINAL DATA:\t" + sb.ToString());

                        sb = new System.Text.StringBuilder();
                        foreach (Test t in dummy_data)
                            sb.Append(t.id + "\t");
                        System.Diagnostics.Debug.WriteLine("ATTEMPTED SORTED DATA:\t" + sb.ToString());

                        sb = new System.Text.StringBuilder();
                        foreach (Test t in sorted_asc)
                            sb.Append(t.id + "\t");
                        System.Diagnostics.Debug.WriteLine("SORTED DATA:\t" + sb.ToString());
                        break;
                    }
                }

                sw.Restart();
                HeapSort(ref dummy_data, SortOrder.Ascending, "id");
                sw.Stop();
                System.Diagnostics.Debug.WriteLine("Heapsort (pre-sorted, ASC->ASC): " + sw.ElapsedMilliseconds + "ms");

                // verify it is sorted
                for (int i = 0; i < dummy_data.Count; i++)
                {
                    if (dummy_data[i].id != sorted_asc[i].id)
                    {
                        System.Diagnostics.Debug.WriteLine("NOT SORTED");
                        break;
                    }
                }

                sw.Restart();
                HeapSort(ref dummy_data, SortOrder.Descending, "id");
                sw.Stop();
                System.Diagnostics.Debug.WriteLine("Heapsort (pre-sorted, ASC->DESC): " + sw.ElapsedMilliseconds + "ms");

                // verify it is sorted
                for (int i = 0; i < dummy_data.Count; i++)
                {
                    if (dummy_data[i].id != sorted_desc[i].id)
                    {
                        System.Diagnostics.Debug.WriteLine("NOT SORTED");
                        break;
                    }
                }

                dummy_data = new List<Test>(all_same_item);
                sw.Restart();
                HeapSort(ref dummy_data, SortOrder.Ascending, "id");
                sw.Stop();
                System.Diagnostics.Debug.WriteLine("Heapsort ASC (All same): " + sw.ElapsedMilliseconds + "ms");

                dummy_data = new List<Test>(dummy_data2);
                sw.Restart();
                HeapSort(ref dummy_data, SortOrder.Descending, "id");
                sw.Stop();
                System.Diagnostics.Debug.WriteLine("Heapsort DESC: " + sw.ElapsedMilliseconds + "ms");

                // verify it is sorted
                for (int i = 0; i < dummy_data.Count; i++)
                {
                    if (dummy_data[i].id != sorted_desc[i].id)
                    {
                        System.Diagnostics.Debug.WriteLine("NOT SORTED");
                        break;
                    }
                }
            }

            #endregion
            
            #region Quicksort

            if (do_quicksort)
            {
                System.Diagnostics.Debug.WriteLine("Starting quicksort..");
                dummy_data = new List<Test>(dummy_data2);
                sw.Restart();
                QuickSort(ref dummy_data, SortOrder.Ascending, "id");
                sw.Stop();
                System.Diagnostics.Debug.WriteLine("Quicksort ASC: " + sw.ElapsedMilliseconds + "ms");

                // verify it is sorted
                for (int i = 0; i < dummy_data.Count; i++)
                {
                    if (dummy_data[i].id != sorted_asc[i].id)
                    {
                        System.Diagnostics.Debug.WriteLine("NOT SORTED");
                        break;
                    }
                }

                sw.Restart();
                QuickSort(ref dummy_data, SortOrder.Ascending, "id");
                sw.Stop();
                System.Diagnostics.Debug.WriteLine("Quicksort (pre-sorted, ASC->ASC): " + sw.ElapsedMilliseconds + "ms");

                // verify it is sorted
                for (int i = 0; i < dummy_data.Count; i++)
                {
                    if (dummy_data[i].id != sorted_asc[i].id)
                    {
                        System.Diagnostics.Debug.WriteLine("NOT SORTED");
                        break;
                    }
                }

                sw.Restart();
                QuickSort(ref dummy_data, SortOrder.Descending, "id");
                sw.Stop();
                System.Diagnostics.Debug.WriteLine("Quicksort (pre-sorted, ASC->DESC): " + sw.ElapsedMilliseconds + "ms");

                // verify it is sorted
                for (int i = 0; i < dummy_data.Count; i++)
                {
                    if (dummy_data[i].id != sorted_desc[i].id)
                    {
                        System.Diagnostics.Debug.WriteLine("NOT SORTED");
                        break;
                    }
                }

                dummy_data = new List<Test>(all_same_item);
                sw.Restart();
                QuickSort(ref dummy_data, SortOrder.Ascending, "id");
                sw.Stop();
                System.Diagnostics.Debug.WriteLine("Quicksort ASC (All same): " + sw.ElapsedMilliseconds + "ms");

                dummy_data = new List<Test>(dummy_data2);
                sw.Restart();
                QuickSort(ref dummy_data, SortOrder.Descending, "id");
                sw.Stop();
                System.Diagnostics.Debug.WriteLine("Quicksort DESC: " + sw.ElapsedMilliseconds + "ms");

                // verify it is sorted
                for (int i = 0; i < dummy_data.Count; i++)
                {
                    if (dummy_data[i].id != sorted_desc[i].id)
                    {
                        System.Diagnostics.Debug.WriteLine("NOT SORTED");
                        break;
                    }
                }
            }

            #endregion
        }

        #endregion

        #endregion
    }
}
