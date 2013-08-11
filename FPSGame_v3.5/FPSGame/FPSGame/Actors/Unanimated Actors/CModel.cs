using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FPSGame
{
    public class CModel : IRenderable
    {
        public Vector3 position { get; set; }
        public Vector3 rotation { get; set; }
        public Vector3 scale { get; set; }
        public Model model { get; private set; }
        private Matrix[] modelTransforms;
        private GraphicsDevice graphicsDevice;

        public CModel(Model model, Vector3 position, Vector3 rotation,
        Vector3 scale, GraphicsDevice graphicsDevice)
        {
            this.model = model;
            modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.graphicsDevice = graphicsDevice;
            generateTags();
        }

        public void Draw(Matrix View, Matrix Projection, Vector3 Cameraposition)
        {
            // Calculate the base transformation by combining
            // translation, rotation, and scaling
            Matrix baseWorld = Matrix.CreateScale(scale)
                * Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X,
                rotation.Z)
                * Matrix.CreateTranslation(position);
            foreach (ModelMesh mesh in model.Meshes)
            {
                Matrix localWorld = modelTransforms[mesh.ParentBone.Index] *
                    baseWorld;
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    Effect effect = meshPart.Effect;
                    if (effect is BasicEffect)
                    {
                        ((BasicEffect)effect).World = localWorld;
                        ((BasicEffect)effect).View = View;
                        ((BasicEffect)effect).Projection = Projection;
                        ((BasicEffect)effect).EnableDefaultLighting();
                    }
                    else
                    {
                        setEffectParameter(effect, "World", localWorld);
                        setEffectParameter(effect, "View", View);
                        setEffectParameter(effect, "Projection", Projection);
                        setEffectParameter(effect, "Cameraposition", Cameraposition);
                    }
                }
                mesh.Draw();
            }
        }

        public void SetModelEffect(Effect effect, bool CopyEffect)
        {
            foreach (ModelMesh mesh in model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    Effect toSet = effect;
                    // Copy the effect if necessary
                    if (CopyEffect)
                        toSet = effect.Clone();
                    MeshTag tag = ((MeshTag)part.Tag);
                    // If this modelMeshPart has a texture, set it to the effect
                    if (tag.Texture != null)
                    {
                        setEffectParameter(toSet, "BasicTexture", tag.Texture);
                        setEffectParameter(toSet, "TextureEnabled", true);
                    }
                    else
                        setEffectParameter(toSet, "TextureEnabled", false);
                    // Set our remaining parameters to the effect
                    setEffectParameter(toSet, "DiffuseColor", tag.Color);
                    setEffectParameter(toSet, "SpecularPower", tag.SpecularPower);
                    part.Effect = toSet;
                }
        }

        // Sets the specified effect parameter to the given effect, if it
        // has that parameter
        void setEffectParameter(Effect effect, string paramName, object val)
        {
            if (effect.Parameters[paramName] == null)
                return;
            if (val is Vector3)
                effect.Parameters[paramName].SetValue((Vector3)val);
            else if (val is bool)
                effect.Parameters[paramName].SetValue((bool)val);
            else if (val is Matrix)
                effect.Parameters[paramName].SetValue((Matrix)val);
            else if (val is Texture2D)
                effect.Parameters[paramName].SetValue((Texture2D)val);
        }

        private void generateTags()
        {
            foreach (ModelMesh mesh in model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                    if (part.Effect is BasicEffect)
                    {
                        BasicEffect effect = (BasicEffect)part.Effect;
                        MeshTag tag = new MeshTag(effect.DiffuseColor, effect.Texture,
                        effect.SpecularPower);
                        part.Tag = tag;
                    }
        }

        // Store references to all of the model's current effects
        public void CacheEffects()
        {
            foreach (ModelMesh mesh in model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                    ((MeshTag)part.Tag).CachedEffect = part.Effect;
        }
        // Restore the effects referenced by the model's cache
        public void RestoreEffects()
        {
            foreach (ModelMesh mesh in model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                    if (((MeshTag)part.Tag).CachedEffect != null)
                        part.Effect = ((MeshTag)part.Tag).CachedEffect;
        }

        public void SetClipPlane(Vector4? Plane)
        {
            foreach (ModelMesh mesh in model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    if (part.Effect.Parameters["ClipPlaneEnabled"] != null)
                        part.Effect.Parameters["ClipPlaneEnabled"].
                        SetValue(Plane.HasValue);
                    if (Plane.HasValue)
                        if (part.Effect.Parameters["ClipPlane"] != null)
                            part.Effect.Parameters["ClipPlane"].SetValue(Plane.Value);
                }
        }
    }
}
