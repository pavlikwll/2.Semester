=== Demo ===

= Test
Cow: Muuuuh...
-> END

=== Angel ===
-> Tutorial1_Start
=Tutorial1_Start
Angel: Hey there wayfarer! I haven't seen you around, are you new here?
*[Yes.] -> start_yes

*[No.] -> start_no

=start_yes
Angel: Thought as much! -> Tutorial1_Start2
=start_no
Angel: A funny one aren't you? Well either way...-> Tutorial1_Start2


= Tutorial1_Start2
You ought to take a tour through the town. We have quite a nice farmland just up north!
~Event("Tutorial1Start")
-> END
= Tutorial1_Running
Angel: Have been to our farmland already? You ought to check it out!
-> END
= Tutorial1_Fulfilled
Angel: And? Did you like it? Sorry I just spent most of my spring freetime on it, so I get pretty exited to show it to a new face.
~Event("Tutorial1End")
->Tutorial2_Start

=Tutorial2_Start
Angel: Oh before I forget! You can rest in the small house just east of us, normally my brother comes over and stays there this time of year, but he isn't doing well and hasn't felt up to it in a little while...
->Tutorial2_Start2
=Tutorial2_Start2
...
->Tutorial2_Start3
=Tutorial2_Start3
... Well... I am sure he wouldn't mind. In any case you should also probably get some rest. Oh and stay away from the caves this late into the evening!
~Event("InvestigateCave")
->END
=Tutorial2_Running
Angel: You should really get some rest, I'll show you the rest of the village tomorrow. Pinky promise!#avatar:angel_happy
Angel: ... Well... I am sure he wouldn't mind. In any case you should also probably get some rest. Oh and stay away from the caves this late into the evening!
You ought to take a tour through the town. We have quite a nice farmland just up north!
->END
