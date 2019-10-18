/********************************************************************************
* ReactPhysics3D physics library, http://www.reactphysics3d.com                 *
* Copyright (c) 2010-2016 Daniel Chappuis                                       *
*********************************************************************************
*                                                                               *
* This software is provided 'as-is', without any express or implied warranty.   *
* In no event will the authors be held liable for any damages arising from the  *
* use of this software.                                                         *
*                                                                               *
* Permission is granted to anyone to use this software for any purpose,         *
* including commercial applications, and to alter it and redistribute it        *
* freely, subject to the following restrictions:                                *
*                                                                               *
* 1. The origin of this software must not be misrepresented; you must not claim *
*    that you wrote the original software. If you use this software in a        *
*    product, an acknowledgment in the product documentation would be           *
*    appreciated but is not required.                                           *
*                                                                               *
* 2. Altered source versions must be plainly marked as such, and must not be    *
*    misrepresented as being the original software.                             *
*                                                                               *
* 3. This notice may not be removed or altered from any source distribution.    *
*                                                                               *
********************************************************************************/

// Libraries
#include "CubesScene.h"

// Namespaces
using namespace openglframework;
using namespace cubesscene;

// Constructor
CubesScene::CubesScene(const std::string& name, EngineSettings& settings)
      : SceneDemo(name, settings, SCENE_RADIUS) {

	
    openglframework::Vector3 center(0, 5, 0);

    // Set the center of the scene
    setScenePosition(center, SCENE_RADIUS);

    // Gravity vector in the dynamics world
    rp3d::Vector3 gravity(0, rp3d::decimal(-9.81), 0);

    rp3d::WorldSettings worldSettings;
    worldSettings.worldName = name;

    // Create the dynamics world for the physics simulation
    mPhysicsWorld = new rp3d::DynamicsWorld(gravity, worldSettings);

    // Create all the cubes of the scene
    for (int i=0; i<NB_CUBES; i++) {

        // Create a cube and a corresponding rigid in the dynamics world
        Box* cube = new Box(BOX_SIZE, BOX_MASS, getDynamicsWorld(), mMeshFolderPath);

        // Set the box color
        cube->setColor(mDemoColors[i % mNbDemoColors]);
        cube->setSleepingColor(mRedColorDemo);
		srand(time(0));
        
        // Change the material properties of the rigid body
        rp3d::Material& material = cube->getRigidBody()->getMaterial();
		
        material.setBounciness(rp3d::decimal(1.0));

        // Add the box the list of box in the scene
        mBoxes.push_back(cube);
        mPhysicsObjects.push_back(cube);
    }

	// ------------------------- FLOOR ----------------------- //

	//glEnable(GL_BLEND);
	//glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	//glDepthMask(GL_FALSE);
    

	// Compute the radius and the center of the scene
    // Create the floor
    mFloor = new Box(FLOOR_SIZE, FLOOR_MASS, getDynamicsWorld(), mMeshFolderPath);
	mFloor->setColor(mTransparentDemo);
	mFloor->setSleepingColor(mTransparentDemo);
	//mFloor_up->setTransform(rp3d::Transform(rp3d::Vector3(0, -40, 0), rp3d::Quaternion::identity()));
    //mFloor->setColor(mGreyColorDemo);
    //mFloor->setSleepingColor(mGreyColorDemo);
	
    // The floor must be a static rigid body
    mFloor->getRigidBody()->setType(rp3d::BodyType::STATIC);
	mPhysicsObjects.push_back(mFloor);
	

	mFloor_up = new Box(FLOOR_SIZE, FLOOR_MASS, getDynamicsWorld(), mMeshFolderPath);
	mFloor_up->setColor(mTransparentDemo);
	mFloor_up->setSleepingColor(mTransparentDemo);
	
	//mFloor->setColor(mGreyColorDemo);
	//mFloor->setSleepingColor(mGreyColorDemo);

	// The floor must be a static rigid body
	mFloor_up->setTransform(rp3d::Transform(rp3d::Vector3(0,80,0), rp3d::Quaternion::identity()));
	mFloor_up->getRigidBody()->setType(rp3d::BodyType::STATIC);
	mPhysicsObjects.push_back(mFloor_up);


	mFloor_backward = new Box(FLOOR_SIZE_1, FLOOR_MASS, getDynamicsWorld(), mMeshFolderPath);
	mFloor_backward->setColor(mTransparentDemo);
	mFloor_backward->setSleepingColor(mTransparentDemo);
	//mFloor->setColor(mGreyColorDemo);
	//mFloor->setSleepingColor(mGreyColorDemo);

	// The floor must be a static rigid body
	mFloor_backward ->setTransform(rp3d::Transform(rp3d::Vector3(0, 40, -40), rp3d::Quaternion::identity()));
	mFloor_backward->getRigidBody()->setType(rp3d::BodyType::STATIC);
	mPhysicsObjects.push_back(mFloor_backward);

	mFloor_forward = new Box(FLOOR_SIZE_1, FLOOR_MASS, getDynamicsWorld(), mMeshFolderPath);
	mFloor_forward->setColor(mTransparentDemo);
	mFloor_forward->setSleepingColor(mTransparentDemo);
	//mFloor->setColor(mGreyColorDemo);
	//mFloor->setSleepingColor(mGreyColorDemo);

	// The floor must be a static rigid body
	mFloor_forward->setTransform(rp3d::Transform(rp3d::Vector3(0, 40, 40), rp3d::Quaternion::identity()));
	mFloor_forward->getRigidBody()->setType(rp3d::BodyType::STATIC);
	mPhysicsObjects.push_back(mFloor_forward);

	mFloor_left = new Box(FLOOR_SIZE_2, FLOOR_MASS, getDynamicsWorld(), mMeshFolderPath);
	mFloor_left->setColor(mTransparentDemo);
	mFloor_left->setSleepingColor(mTransparentDemo);
	//mFloor->setColor(mGreyColorDemo);
	//mFloor->setSleepingColor(mGreyColorDemo);

	// The floor must be a static rigid body
	mFloor_left->setTransform(rp3d::Transform(rp3d::Vector3(40, 40, 0), rp3d::Quaternion::identity()));
	mFloor_left->getRigidBody()->setType(rp3d::BodyType::STATIC);
	mPhysicsObjects.push_back(mFloor_left);

	mFloor_right = new Box(FLOOR_SIZE_2, FLOOR_MASS, getDynamicsWorld(), mMeshFolderPath);
	mFloor_right->setColor(mTransparentDemo);
	mFloor_right->setSleepingColor(mTransparentDemo);
	//mFloor->setColor(mGreyColorDemo);
	//mFloor->setSleepingColor(mGreyColorDemo);

	// The floor must be a static rigid body
	mFloor_right->setTransform(rp3d::Transform(rp3d::Vector3(-40, 40, 0), rp3d::Quaternion::identity()));
	mFloor_right->getRigidBody()->setType(rp3d::BodyType::STATIC);
	mPhysicsObjects.push_back(mFloor_right);

	//glDepthMask(GL_TRUE);
	

    // Get the physics engine parameters
    mEngineSettings.isGravityEnabled = getDynamicsWorld()->isGravityEnabled();
    rp3d::Vector3 gravityVector = getDynamicsWorld()->getGravity();
    //mEngineSettings.gravity = openglframework::Vector3(gravityVector.x, gravityVector.y, gravityVector.z);
	mEngineSettings.gravity = openglframework::Vector3(0, 0, 0);
    mEngineSettings.isSleepingEnabled = getDynamicsWorld()->isSleepingEnabled();
    mEngineSettings.sleepLinearVelocity = getDynamicsWorld()->getSleepLinearVelocity();
    mEngineSettings.sleepAngularVelocity = getDynamicsWorld()->getSleepAngularVelocity();
    mEngineSettings.nbPositionSolverIterations = getDynamicsWorld()->getNbIterationsPositionSolver();
    mEngineSettings.nbVelocitySolverIterations = getDynamicsWorld()->getNbIterationsVelocitySolver();
    mEngineSettings.timeBeforeSleep = getDynamicsWorld()->getTimeBeforeSleep();
	
}

// Destructor
CubesScene::~CubesScene() {

    // Destroy all the cubes of the scene
    for (std::vector<Box*>::iterator it = mBoxes.begin(); it != mBoxes.end(); ++it) {

        // Destroy the corresponding rigid body from the dynamics world
        getDynamicsWorld()->destroyRigidBody((*it)->getRigidBody());

        // Destroy the cube
        delete (*it);
    }

    // Destroy the rigid body of the floor
    getDynamicsWorld()->destroyRigidBody(mFloor->getRigidBody());

    // Destroy the floor
    delete mFloor;

    // Destroy the dynamics world
    delete getDynamicsWorld();
}

// Reset the scene
void CubesScene::reset() {

    float radius = 2.0f;

    // Create all the cubes of the scene
    std::vector<Box*>::iterator it;
    int i = 0;
    for (it = mBoxes.begin(); it != mBoxes.end(); ++it) {

        // Position of the cubes
       float angle = i * 30.0f;
       rp3d::Vector3 position(radius * std::cos(angle),
                              10 + i * (BOX_SIZE.y + 0.3f),
                              0);

       (*it)->setTransform(rp3d::Transform(position, rp3d::Quaternion::identity()));
	   (*it)->getRigidBody()->setLinearVelocity(rp3d::Vector3(rand() % 10, rand() % 12, rand() % 10));

       i++;
    }

    mFloor->setTransform(rp3d::Transform(rp3d::Vector3::zero(), rp3d::Quaternion::identity()));
}
