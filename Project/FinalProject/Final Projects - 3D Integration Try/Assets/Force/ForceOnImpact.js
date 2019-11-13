#pragma strict
var replace : Transform;
var MaxForce : float = 10;
function OnCollisionEnter(col : Collision){


if(col.impactForceSum.y >= MaxForce){
Instantiate(replace, transform.position, transform.rotation);
Destroy(gameObject);


}
}