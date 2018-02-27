using IBALib.Interfaces;
using IBALib.Types;
using Processing.ImageProcessing.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Processing.ImageProcessing
{
    public class ImageProcessor<T> : IImageProcessor<T>
    {
        private enum STATE
        {
            CREATED,
            MODIFIED,
            IN_PROGRESS,
            COMPLETED
        }

        private List<ImageWrapper<T>> _images;
        private List<IImageProcessorCommand> _commands;
        private STATE _state;
        private ImageWrapper<T> _result;

        public ImageWrapper<T> Result
        {
            get
            {
                if (_state != STATE.COMPLETED) throw new InvalidOperationException("The is no result");
                return _result;
            }
        }

        public ImageProcessor()
        {
            _images = new List<ImageWrapper<T>>();
            _commands = new List<IImageProcessorCommand>();
            _state = STATE.CREATED;
        }

        public void AddCommand(IImageProcessorCommand command)
        {
            _commands.Add(command);
            SwitchToModifiedState();
        }

        public void RemoveCommand(IImageProcessorCommand command)
        {
            _commands.Remove(command);
            SwitchToModifiedState();
        }

        public void AddImage(ImageWrapper<T> image)
        {
            _images.Add(image);
            SwitchToModifiedState();
        }

        public void AddImage(IEnumerable<ImageWrapper<T>> images)
        {
            _images.AddRange(images);
            SwitchToModifiedState();
        }

        public void RemoveImage(ImageWrapper<T> image)
        {
            _images.Remove(image);
            SwitchToModifiedState();
        }

        public void Process()
        {
            switch (_state)
            {
                case STATE.MODIFIED:
                    _state = STATE.IN_PROGRESS;
                    foreach (var command in _commands)
                    {
                        Log.Debug($"Begin performing {command.GetType().Name}");
                        _images = new List<ImageWrapper<T>>(command.Perform(_images));
                        Log.Debug($"End performing {command.GetType().Name}");
                    }
                    _state = STATE.COMPLETED;
                    _result = _images[0];
                    break;
                case STATE.CREATED:
                case STATE.IN_PROGRESS:
                case STATE.COMPLETED:
                    throw new InvalidOperationException($"Cannot process images in {_state} state");
                default:
                    break;
            }
        }

        private void SwitchTo(STATE state) => _state = state;

        private void SwitchToModifiedState() => SwitchTo(STATE.MODIFIED);
 
    }
}
