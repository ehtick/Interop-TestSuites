namespace Microsoft.Protocols.TestSuites.MS_OXCTABL
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Protocols.TestSuites.Common;

    /// <summary>
    /// For Rule Action format
    /// </summary>
    public class RuleAction
    {
        /// <summary>
        /// Specifies Actions in the Rule
        /// </summary>
        private ActionBlock[] actions;

        /// <summary>
        /// Type of COUNT
        /// </summary>
        private Count countType;

        /// <summary>
        /// Specifies the number of ActionBlocks that are packed in this buffer. This number MUST be greater than zero. In extended rule, its type is uint. Otherwise, its type is ushort.
        /// </summary>
        private object actionsNumber;

        /// <summary>
        /// Initializes a new instance of the RuleAction class.
        /// </summary>
        public RuleAction()
        {
            this.countType = Count.TwoBytesCount;
        }

        /// <summary>
        /// Initializes a new instance of the RuleAction class.
        /// </summary>
        /// <param name="countType">The COUNT Type of this class</param>
        public RuleAction(Count countType)
        {
            this.countType = countType;
        }

        /// <summary>
        /// Gets or sets Actions in the Rule 
        /// </summary>
        public ActionBlock[] Actions
        {
            get { return this.actions; }
            set { this.actions = value; }
        }

        /// <summary>
        /// Gets type of Count
        /// </summary>
        public Count CountType
        {
            get { return this.countType; }
        }

        /// <summary>
        /// Gets or sets the number of ActionBlocks that are packed in this buffer. This number MUST be greater than zero. In extended rule, its type is uint. Otherwise, its type is ushort.
        /// </summary>
        public object NoOfActions
        {
            get
            {
                return this.actionsNumber;
            }

            set
            {
                if (this.CountType == Count.TwoBytesCount)
                {
                    if (value is int)
                    {
                        this.actionsNumber = (ushort)(int)value;
                    }
                    else
                    {
                        this.actionsNumber = (ushort)value;
                    }
                }
                else
                {
                    if (value is int)
                    {
                        this.actionsNumber = (uint)(int)value;
                    }
                    else
                    {
                        this.actionsNumber = (uint)value;
                    }
                }
            }
        }

        /// <summary>
        /// Get the total Size of ActionData
        /// </summary>
        /// <returns>The Size of RuleAction buffer</returns>
        public int Size()
        {
            int result = 0;
            if (this.CountType == Count.TwoBytesCount)
            {
                // Length of NoOfActions
                result = 2;
            }
            else if (this.CountType == Count.FourBytesCount)
            {
                // Length of NoOfActions
                result = 4;
            }

            foreach (ActionBlock actionBlock in this.Actions)
            {
                result += actionBlock.Size();
            }

            return result;
        }

        /// <summary>
        /// Get serialized byte array for this structure
        /// </summary>
        /// <returns>Serialized byte array</returns>
        public byte[] Serialize()
        {
            List<byte> result = new List<byte>();
            if (this.CountType == Count.TwoBytesCount)
            {
                result.AddRange(BitConverter.GetBytes((ushort)this.NoOfActions));
            }
            else if (this.CountType == Count.FourBytesCount)
            {
                result.AddRange(BitConverter.GetBytes((uint)this.NoOfActions));
            }

            foreach (ActionBlock actionBlock in this.Actions)
            {
                result.AddRange(actionBlock.Serialize());
            }

            return result.ToArray();
        }

        /// <summary>
        /// Parse method to obtain current structure from byte array
        /// </summary>
        /// <param name="buffer">Byte array data</param>
        /// <returns>Bytes count that deserialized in buffer</returns>
        public uint Deserialize(byte[] buffer)
        {
            BufferReader bufferReader = new BufferReader(buffer);
            uint count = 0;
            if (this.CountType == Count.TwoBytesCount)
            {
                this.NoOfActions = bufferReader.ReadUInt16();
                this.Actions = new ActionBlock[(ushort)this.NoOfActions];
                count = (uint)(ushort)this.NoOfActions;
            }
            else if (this.CountType == Count.FourBytesCount)
            {
                this.NoOfActions = bufferReader.ReadUInt32();
                this.Actions = new ActionBlock[(uint)this.NoOfActions];
                count = (uint)this.NoOfActions;
            }

            uint totalBytes = bufferReader.Position;
            byte[] tmpArray = bufferReader.ReadToEnd();
            uint bytesCount = 0;
            for (uint i = 0; i < count; i++)
            {
                bufferReader = new BufferReader(tmpArray);
                tmpArray = bufferReader.ReadBytes(bytesCount, (uint)(tmpArray.Length - bytesCount));
                this.Actions[i] = new ActionBlock(this.CountType);
                bytesCount = this.Actions[i].Deserialize(tmpArray);
                totalBytes += bytesCount;
            }

            return totalBytes;
        }
    }
}