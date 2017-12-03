using System;
using System.Collections.Generic;

namespace Engine.Models
{
    internal class JobQueue
    {
        /* #################################################################### */
        /* #                         CONSTANT FIELDS                          # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                              FIELDS                              # */
        /* #################################################################### */

        private readonly Queue<Job> _jobQueue;
        private Action<Job> cbJobCreated;

        /* #################################################################### */
        /* #                           CONSTRUCTORS                           # */
        /* #################################################################### */

        public JobQueue()
        {
            _jobQueue = new Queue<Job>();
        }

        /* #################################################################### */
        /* #                             DELEGATES                            # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                            PROPERTIES                            # */
        /* #################################################################### */

        /* #################################################################### */
        /* #                              METHODS                             # */
        /* #################################################################### */

        public void Enqueue(Job j)
        {
            _jobQueue.Enqueue(j);
            cbJobCreated?.Invoke(j);
        }

        public Job Dequeue()
        {
            if (_jobQueue.Count == 0)
            {
                return null;
            }

            return _jobQueue.Dequeue();
        }

        public void RegisterJobCreationCallback(Action<Job> cb)
        {
            cbJobCreated += cb;
        }

        public void UnRegisterJobCreationCallback(Action<Job> cb)
        {
            cbJobCreated -= cb;
        }
    }
}