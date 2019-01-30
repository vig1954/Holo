using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Common;
using Infrastructure;
using Processing;

namespace UserInterface.DataEditors.Renderers.Shaders
{
    public class ShaderProvider
    {
        private Dictionary<int, List<IImageShader>> _cachedShaders = new Dictionary<int, List<IImageShader>>();

        public T Get<T>() where T : IImageShader
        {
            return (T) Get(typeof(T));
        }

        public IImageShader Get(Type t)
        {
            if (!t.IsClass || t.IsAbstract)
                throw new InvalidOperationException();

            // todo убрать логику в какой-нибудь провайдер
            var editorId = DataEditorView.CurrentlyUpdatingEditorId ?? Singleton.Get<DataEditorManager>().GetActive()?.Id ?? throw new InvalidOperationException();
            if (!_cachedShaders.ContainsKey(editorId))
                _cachedShaders.Add(editorId, new List<IImageShader>());

            if (_cachedShaders[editorId].Any(t.IsInstanceOfType))
                return _cachedShaders[editorId].Single(t.IsInstanceOfType);

            var shader = (IImageShader)Activator.CreateInstance(t);
            _cachedShaders[editorId].Add(shader);

            return shader;
        }

        public IReadOnlyCollection<IImageShader> GetFor(IImageHandler imageHandler)
        {
            var shaderTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IImageShader).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

            var specificShaderTypes = shaderTypes.Where(t => t.GetCustomAttribute<TargetImageAttribute>()?.ImageFormat == imageHandler.Format).ToArray();
            if (specificShaderTypes.Any())
                return specificShaderTypes.Select(Get).ToArray();

            var commonShaderTypes = shaderTypes.Where(t => t.GetCustomAttribute<TargetImageAttribute>() == null).ToArray();
            if (commonShaderTypes.Any())
                return commonShaderTypes.Select(Get).ToArray();

            return Enumerable.Empty<IImageShader>().ToArray();
        }
    }
}
