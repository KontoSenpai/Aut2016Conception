using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
		private bool canJump = false;
		private bool hasCollide = false;

        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {
			/*if (canJump) {
				// Read the jump input in Update so button presses aren't missed.

				m_Jump = true;
			} 
			else {
				m_Jump = false;
			}*/
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
			
            m_Character.Move(h, crouch, m_Jump);
			m_Jump = false;
    
        }

		public void OnCollisionEnter2D(Collision2D coll) {
			if (coll.gameObject.tag == "Sol" && !hasCollide) {
				hasCollide = true;
				m_Jump = true;

			}
		}

		public void OnCollisionExit2D(Collision2D coll) {
			if (coll.gameObject.tag == "Sol" && hasCollide) {
				hasCollide = false;
				m_Jump = false;
			}
		}
	}
}
