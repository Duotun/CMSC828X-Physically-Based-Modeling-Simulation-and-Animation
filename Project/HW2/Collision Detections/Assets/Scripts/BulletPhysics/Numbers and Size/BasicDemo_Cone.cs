using System;
using BulletSharp;
using BulletSharp.Math;
using DemoFramework;
using BulletSharpExamples;

namespace BasicDemo_Cone
{
    public class BasicDemo_Cone : Demo
    {
        Vector3 eye = new Vector3(100, 40, 10);
        Vector3 target = new Vector3(0, 5, -4);

        // create 125 (5x5x5) dynamic objects
        const int ArraySizeX = 3, ArraySizeY = 5, ArraySizeZ = 5;

        // scaling of the objects (0.1 = 20 centimeter boxes )
        const float StartPosX = -5;
        const float StartPosY = -5;
        const float StartPosZ = -3;

        float mass = 1f;
        Vector3 gravity = new Vector3(0, -9.8f, 0f);
        int size;
        public BasicDemo_Cone(float masspass, Vector3 gravitypass,int sizepass)
        {
            this.mass = masspass;
            this.gravity = gravitypass;
            this.size = sizepass;

        }
        public BasicDemo_Cone()
        {


        }

        protected override void OnInitialize()
        {
            Freelook.SetEyeTarget(eye, target);

            Graphics.SetFormText("BulletSharp - Basic Demo_Cone");
        }

        protected override void OnInitializePhysics()
        {
            // collision configuration contains default setup for memory, collision setup
            CollisionConf = new DefaultCollisionConfiguration();
            Dispatcher = new CollisionDispatcher(CollisionConf);

            Broadphase = new DbvtBroadphase();

            World = new DiscreteDynamicsWorld(Dispatcher, Broadphase, null, CollisionConf);
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
            float mass = this.mass;

            ConeShape colShape = new ConeShape(0.5f*this.size, 1*this.size);


            CollisionShapes.Add(colShape);
            Vector3 localInertia = colShape.CalculateLocalInertia(mass);

            const float startX = StartPosX - ArraySizeX / 2;
            const float startY = StartPosY;
            const float startZ = StartPosZ - ArraySizeZ / 2;

            RigidBodyConstructionInfo rbInfo =
                new RigidBodyConstructionInfo(mass, null, colShape, localInertia);


           
            for (int k = 0; k < ArraySizeY; k++)
            {
                for (int i = 0; i < ArraySizeX; i++)
                {
                    for (int j = 0; j < ArraySizeZ; j++)
                    {
                        Matrix startTransform = Matrix.Translation(
                            2 * i *this.size+ startX,
                            2 * k * this.size + startY,
                            2 * j * this.size + startZ
                        );
                        // using motionstate is recommended, it provides interpolation capabilities
                        // and only synchronizes 'active' objects
                        rbInfo.MotionState = new DefaultMotionState(startTransform);

                        RigidBody body = new RigidBody(rbInfo);
                        body.Restitution = 1;
                        // make it drop from a height
                        body.Translate(new Vector3(0, 20, 0));

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
            using (Demo demo = new BasicDemo_Cone())
            {
                GraphicsLibraryManager.Run(demo);
            }
        }
    }
}
