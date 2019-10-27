using System;
using BulletSharp;
using BulletSharp.Math;
using DemoFramework;
using BulletSharpExamples;

namespace BasicDemo_Mixed
{
    public class BasicDemo_Mixed : Demo
    {
        Vector3 eye = new Vector3(100, 40, 10);
        Vector3 target = new Vector3(0, 5, -4);

        // create 125 (5x5x5) dynamic objects
        const int ArraySizeX = 3, ArraySizeY = 3, ArraySizeZ = 3;

        // scaling of the objects (0.1 = 20 centimeter boxes )
        const float StartPosX = -5;
        const float StartPosY = -5;
        const float StartPosZ = -3;

        float mass = 1f;
        Vector3 gravity = new Vector3(0, -9.8f, 0f);
        int size;
        public BasicDemo_Mixed(float masspass, Vector3 gravitypass,int sizepass)
        {
            this.mass = masspass;
            this.gravity = gravitypass;
            this.size = sizepass;

        }
        public BasicDemo_Mixed()
        {


        }
        protected override void OnInitialize()
        {
            Freelook.SetEyeTarget(eye, target);

            Graphics.SetFormText("BulletSharp - Basic Demo_Mixed");
        }

        protected override void OnInitializePhysics()
        {
            // collision configuration contains default setup for memory, collision setup
            CollisionConf = new DefaultCollisionConfiguration();
            Dispatcher = new CollisionDispatcher(CollisionConf);

            Broadphase = new DbvtBroadphase();
            //AxisSweep3 s = new AxisSweep3(Vector3.One, new Vector3(50, 50, 50));

            World = new DiscreteDynamicsWorld(Dispatcher, Broadphase, null, CollisionConf);
            //World = new DiscreteDynamicsWorld(Dispatcher, s, null, CollisionConf);
            World.Gravity = this.gravity;

            // create the ground
            BoxShape groundShape = new BoxShape(50, 1, 50);
            //groundShape.InitializePolyhedralFeatures();
            //CollisionShape groundShape = new StaticPlaneShape(new Vector3(0,1,0), 50);

            CollisionShapes.Add(groundShape);
            CollisionObject ground = LocalCreateRigidBody(0, Matrix.Translation(0, -50, 0), groundShape);
            ground.UserObject = "Ground";
            ground.Restitution = 1.0f;

            // create the ground

            //groundShape.InitializePolyhedralFeatures();
            //CollisionShape groundShape = new StaticPlaneShape(new Vector3(0,1,0), 50);

            CollisionShapes.Add(groundShape);
            CollisionObject groundup = LocalCreateRigidBody(0, Matrix.Translation(0, 50, 0), groundShape);
            groundup.UserObject = "Ground";
            groundup.Restitution = 1.0f;

            BoxShape groundShape2 = new BoxShape(1, 50, 50);
            CollisionShapes.Add(groundShape2);
            CollisionObject groundleft = LocalCreateRigidBody(0, Matrix.Translation(-50, 0, 0), groundShape2);
            groundleft.UserObject = "Ground";
            groundleft.Restitution = 1.0f;

            CollisionShapes.Add(groundShape2);
            CollisionObject groundright = LocalCreateRigidBody(0, Matrix.Translation(50, 0, 0), groundShape2);
            groundright.UserObject = "Ground";
            groundright.Restitution = 1.0f;

            BoxShape groundShape3 = new BoxShape(50, 50, 1);
            CollisionShapes.Add(groundShape3);
            CollisionObject groundforward = LocalCreateRigidBody(0, Matrix.Translation(0, 0, -50), groundShape3);
            groundforward.UserObject = "Ground";
            groundforward.Restitution = 1.0f;

            CollisionShapes.Add(groundShape3);
            CollisionObject grounddown = LocalCreateRigidBody(0, Matrix.Translation(0, 0, 50), groundShape3);
            grounddown.UserObject = "Ground";
            grounddown.Restitution = 1.0f;



            // create a few dynamic rigidbodies
            CollisionShape[] colShapes = {
                new SphereShape(1*this.size),
                new CapsuleShape(0.5f*this.size,1*this.size),
                new CapsuleShapeX(0.5f*this.size,1*this.size),
                new CapsuleShapeZ(0.5f*this.size,1*this.size),
                new ConeShape(0.5f*this.size,1*this.size),
                new ConeShapeX(0.5f*this.size,1*this.size),
                new ConeShapeZ(0.5f*this.size,1*this.size),
                new CylinderShape(new Vector3(0.5f*this.size,1*this.size,0.5f*this.size)),
                new CylinderShapeX(new Vector3(1f*this.size,0.5f*this.size,0.5f*this.size)),
                new CylinderShapeZ(new Vector3(0.5f*this.size,0.5f*this.size,1*this.size)),
            };
            foreach (var collisionShape in colShapes)
            {
                CollisionShapes.Add(collisionShape);
            }

            // create a few dynamic rigidbodies
            float mass = this.mass;

            CollisionShape colShape = new BoxShape(1);
            CollisionShapes.Add(colShape);
            Vector3 localInertia = colShape.CalculateLocalInertia(mass);

            var rbInfo = new RigidBodyConstructionInfo(mass, null, null, localInertia);

            const float startX = StartPosX - ArraySizeX / 2;
            const float startY = StartPosY;
            const float startZ = StartPosZ - ArraySizeZ / 2;


            int shapeIndex = 0;

            for (int k = 0; k < ArraySizeY; k++)
            {
                for (int i = 0; i < ArraySizeX; i++)
                {
                    for (int j = 0; j < ArraySizeZ; j++)
                    {
                        Matrix startTransform = Matrix.Translation(
                            2 * i*this.size + startX,
                            2 * k * this.size + startY,
                            2 * j * this.size + startZ
                        );
                        // using motionstate is recommended, it provides interpolation capabilities
                        // and only synchronizes 'active' objects
                        shapeIndex++;

                        // using motionstate is recommended, it provides interpolation capabilities
                        // and only synchronizes 'active' objects
                        rbInfo.MotionState = new DefaultMotionState(startTransform);
                        rbInfo.CollisionShape = colShapes[shapeIndex % colShapes.Length];

                        RigidBody body = new RigidBody(rbInfo);
                        body.Friction = 1;
                        body.RollingFriction = 0.3f;
                        body.SetAnisotropicFriction(colShape.AnisotropicRollingFrictionDirection, AnisotropicFrictionFlags.RollingFriction);
                        body.Restitution = 1f;
                        World.AddRigidBody(body);
                    }
                }
            }

            rbInfo.Dispose();
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            using (Demo demo = new BasicDemo_Mixed())
            {
                GraphicsLibraryManager.Run(demo);
            }
        }
    }
}
