namespace Boids


//TODO:  Create arraylist like object
//USING: https://processing.org/examples/flocking.html

(*
//CODE will be in unity calling
Flock flock;

void setup() {
  size(640, 360);
  flock = new Flock();
  // Add an initial set of boids into the system
  for (int i = 0; i < 150; i++) {
    flock.addBoid(new Boid(width/2,height/2));
  }
}

void draw() {
  background(50);
  flock.run();
}

// Add a new boid into the System
void mousePressed() {
  flock.addBoid(new Boid(mouseX,mouseY));
}

*)

type Vector(x : float, y : float) =
    //We have the X and Y values be private mutables
    let mutable XValue = x      //C++/Java equivalent private float X
    let mutable YValue = y      //C++/Java equivalent private float Y
    
    //In order to modify the variables we need to use with get() and set()
    member this.X                                
        with get() = XValue                //C++/Java equivalent vector.X.get()
        and set(value) = XValue <- value   //C++/Java equivalent vector.X.set(float)

    member this.Y
        with get() = YValue                //C++/Java equivalent vector.Y.get()
        and set(value) = YValue <- value   //C++/Java equivalent vector.Y.set(float)

    //Public add/sub/limit functions
    member this.add(v : Vector) =          //C++/Java equivalent vector.add(vector2)
         Vector(x + v.X, y + v.Y)
    
    member this.sub(v : Vector) =          //C++/Java equivalent vector.sub(vector2)
         Vector(x - v.X, y - v.Y)

    member this.sub(first : Vector, second : Vector) =
         Vector(first.X - second.X, first.Y - second.Y)

    member this.mult(s : int) =  //C++/Java equivalent vector.mult(int)
         Vector(x * float s, y * float s)

    member this.mult(s : double) =  //C++/Java equivalent vector.mult(double)
         Vector(x * s, y * s)
    
    member this.mult(s : float) =  //C++/Java equivalent vector.mult(float)
         Vector(x * s, y * s)

    //We can use the assignment ( <- )as we have defined the get and set previously
    member this.limit(s : int) =    //C++/Java equivalent vector.limit(int)
        if this.X > float s then    
             this.X <- float s     //C++/Java equivalent this.X.set(s)
        if y > float s then 
            this.Y <- float s     //C++/Java equivalent this.Y.set(s)

    member this.normalize() = 
            this.Y <- 0.0
        

    //Static members are considered a "class function" meaning that they do not require
    //An instance of the class to run 
    static member add(v1 : Vector, v2 : Vector) =   //C++/Java equivalent Vector.add(vector1, vector2)
        Vector(v1.X + v2.X, v1.Y + v2.Y)

    static member sub(v1 : Vector, v2 : Vector) =   //C++/Java equivalent Vector.sub(vector1, vector2)
        Vector(v1.X - v2.X, v1.Y - v2.Y)


// The Flock (a list of Boid objects)

type Flock () = 
    member this.Boids = ArrayList<Boid> boids; // An ArrayList for all the boids

    member this.run() =
        for Boid b in Boids
            do b.run Boids  // Passing the entire list of boids to each boid individually

    member this.addBoid(b : boid)
        Boids.add(b)

// The Boid class

type Boid (x : float, y: float) =

    member this.location = Vector(x , y)
    member this.r = 2.0
    member this.maxspeed = 2
    member this.maxforce = 0.03 
    member this.velocity = Vector(cos(random(3.14)), sin(random(3.14)))
    member this.acceleration = Vector(0.0 , 0.0)

    member this.run(boids : ArrayList<Boid> ) =
        this.flock(boids)
        this.update
        (*
        //call to run in unity?
        borders();
        render();
        *)
  

    member this.applyForce(force : Vector) =
        // We could add mass here if we want A = F / M
        this.acceleration.add(force);

      // We accumulate a new acceleration each time based on three rules
    member this.flock(ArrayList<Boid> boids) =
       //May have to just do calls and do the applyForce after the completion of each function
        //Declaring private mutable variables
        let mutable sep = this.separate(boids)    // Separation
        let mutable ali = this.align(boids)   // Alignment
        let mutable coh = this.cohesion(boids)  // Cohesion
        // Arbitrarily weight these forces
        sep.mult(1.5 : double);
        ali.mult(1.0);
        coh.mult(1.0);
        // Add the force vectors to acceleration
        this.applyForce(sep);
        this.applyForce(ali);
        this.applyForce(coh);

      // Method to update location
    member this.update() =
        // Update velocity
        this.velocity.add(this.acceleration);
        // Limit speed
        this.velocity.limit(this.maxspeed);
        this.location.add(this.velocity);
        // Reset accelertion to 0 each cycle
        this.acceleration.mult(0);


    // A method that calculates and applies a steering force towards a target
    // STEER = DESIRED MINUS VELOCITY
    member this.seek(target : Vector) =
        let desired = Vector.sub(target, this.location)  // A vector pointing from the location to the target
        // Scale to maximum speed
        desired.normalize()
        desired.mult(maxspeed)

        // Above two lines of code below could be condensed with new PVector setMag() method
        // Not using this method until Processing.js catches up
        // desired.setMag(maxspeed);

        // Steering = Desired minus Velocity
        let steer = Vector.sub(desired, velocity);
        steer.limit(maxforce);  // Limit to maximum steering force

//  void render() {
//    // Draw a triangle rotated in the direction of velocity
//    float theta = velocity.heading2D() + radians(90);
//    // heading2D() above is now heading() but leaving old syntax until Processing.js catches up
//    
//    fill(200, 100);
//    stroke(255);
//    pushMatrix();
//    translate(location.x, location.y);
//    rotate(theta);
//    beginShape(TRIANGLES);
//    vertex(0, -r*2);
//    vertex(-r, r*2);
//    vertex(r, r*2);
//    endShape();
//    popMatrix();
//  }
//
//  // Wraparound
//  void borders() {
//    if (location.x < -r) location.x = width+r;
//    if (location.y < -r) location.y = height+r;
//    if (location.x > width+r) location.x = -r;
//    if (location.y > height+r) location.y = -r;
//  }

  // Separation
  // Method checks for nearby boids and steers away
    member this.separate (ArrayList<Boid> boids)  =
        let desiredseparation = 25.0f;
        let steer = new Vector(0, 0, 0);
        let count = 0;
        // For every boid in the system, check if it's too close
        for other in boids do
            double d = Vector.dist(location, other.location)
            // If the distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
            if ((d > 0) && (d < desiredseparation)) then
                // Calculate vector pointing away from neighbor
                let diff = Vector.sub(location, other.location)
                diff.normalize()
                diff.div(d)      // Weight by distance
                steer.add(diff)
                count++            // Keep track of how many


        // Average -- divide by how many
        if (count > 0) then
            steer.div((float)count)
        

        // As long as the vector is greater than 0
        if (steer.mag() > 0) then
          // First two lines of code below could be condensed with new PVector setMag() method
          // Not using this method until Processing.js catches up
          // steer.setMag(maxspeed);

          // Implement Reynolds: Steering = Desired - Velocity
          steer.normalize() 
          steer.mult(maxspeed)
          steer.sub(velocity)
          steer.limit(maxforce)
        
        steer

    // Alignment
    // For every nearby boid in the system, calculate the average velocity
    member this.align (ArrayList<Boid> boids) =
        int this.neighbordist = 50
        Vector this.sum = Vector(0, 0)
        let mutable this.count = 0
        for Boid other in boids do
            float d = Vector.dist(this.location, other.location);
            if ((d > 0) && (d < neighbordist)) then
                sum.add(other.velocity)
                this.count++


        if count > 0 then
          sum.div((float)count)
          // First two lines of code below could be condensed with new PVector setMag() method
          // Not using this method until Processing.js catches up
          // sum.setMag(maxspeed);
          // Implement Reynolds: Steering = Desired - Velocity
          sum.normalize()
          sum.mult(maxspeed)
          Vector steer = Vector.sub(sum, velocity)
          steer.limit(maxforce)

        else 
          Vector(0, 0);


    // Cohesion
    // For the average location (i.e. center) of all nearby boids, calculate steering vector towards that location
    member this.cohesion (ArrayList<Boid> boids) 
        float neighbordist = 50
        let sum = new Vector(0, 0)   // Start with empty vector to accumulate all locations
        let count = 0;
        for other in boids do
            float d = PVector.dist(location, other.location);
            if ((d > 0) && (d < neighbordist)) then
                sum.add(other.location) // Add location
                count++

        if (count > 0) then
            sum.div(count)
            seek(sum)  // Steer towards the location

        else 
            Vector(0, 0)
