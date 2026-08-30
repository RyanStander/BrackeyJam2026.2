VAR greivance = 0
VAR fightCount = 0

// hub checks
VAR why = true
VAR money = true

// talk_3 checks
VAR tellTruth = false
VAR knife = false

-> intro

=== intro ===
// black screen
John Apocalypse: I knew this day would come. The Pit doesn't wait for anyone, and neither does the debt collector back home.
John Apocalypse: I need the money. That's the only reason I'm standing in this cage of rusted iron and cheering strangers.

GUNSLINGER: Hey! You sleeping?
GUNSLINGER: It's our turn now. I hope you know how to fight. Do you?

+ [Yes.]John Apocalypse: Yes.
    -> fight
+ [No...]John Apocalypse: No...
    -> tutorial

=== fight ===
GUNSLINGER: I like your fire! Let's do this.
// fight sequence
# gate:fight
-> talk_1

=== tutorial ===
// ring bell sound
GUNSLINGER: Huh. Guess I'll teach you in the arena, then. Don't make me wait.
// showing movement icon
// entering the arena
GUNSLINGER: Walking, you can figure out yourself.
GUNSLINGER: I'll draw their attention from range. You use the strike button when I open them up.
// fight until special attack ready
GUNSLINGER: Now's your chance! Give them no mercy!
// fight ends
# gate:fight
-> talk_1

=== talk_1 ===
GUNSLINGER: You did good work out there. You fight often, or was that beginner's luck?

+ [Tell him about your skills]
    John Apocalypse: In the past, I fought a lot. To survive, not for sport.
    John Apocalypse: (I hope he doesn't push for more. Time to change the subject.)
    John Apocalypse: You're not bad yourself, for what it's worth.
    GUNSLINGER: Hah, as you said - it's essential to survive down here.
    GUNSLINGER: But with the prize money, surviving gets a little easier for both of us.
    ~ greivance += 1
    -> dialog_hub

+ [Be sarcastic]
    John Apocalypse: Duh. Otherwise why would I pick this lovely career path?
    GUNSLINGER: You think this is a joke?
    GUNSLINGER: With that attitude, your ass won't last a week down here.
    John Apocalypse: Bet. I'm walking out of this place alive.
    ~ greivance -= 1
    -> dialog_hub

* [Be suspicious]
    John Apocalypse: (Can't give him ammunition. Keep it vague.)
    John Apocalypse: Not really. Why do you want to know?
    GUNSLINGER: Can't I be curious about my own teammate?
    John Apocalypse: ...Fair enough.
    -> dialog_hub

=== dialog_hub ===
John Apocalypse: {&Free time, I guess?|Should I break the silence?|We've got some time before the next fight. Might as well talk.}

+ {why} [Why are you even here?]John Apocalypse: Why are you even here?
    { greivance >= 2:
        GUNSLINGER: ...Fine. You've earned that much. I owed money to the wrong people. The Wardens buy debts like this - you fight, you clear it, or you don't leave. Simple as that.
        ~ why = false
    - else:
        GUNSLINGER: To win. There's nothing else you need to know.
    }
    -> dialog_hub

* [Do you know the purpose of this place?]John Apocalypse: Do you know the purpose of this place?
    GUNSLINGER: The Pit? Officially it's "entertainment for the upper city." Unofficially it's how the Wardens turn desperate people into a paycheck for people who never have to bleed for theirs.
    GUNSLINGER: Win enough fights, you get your freedom bought back. Lose, and you're just another name they stop bothering to remember.
    -> dialog_hub

+ {money} [What will you do with the money, once we're out?]John Apocalypse: What will you do with the money, once we're out?
    { greivance >= 3:
        GUNSLINGER: There's a kid outside these walls. Not mine, but might as well be. I promised I'd come back with enough to get her somewhere safe.
        ~ money = false
    - else:
        GUNSLINGER: That's not your business. Don't ask again.
    }
    -> dialog_hub

+ [Wait for the next fight.]
    // sound + fade to black and back
    John Apocalypse: It's time. Let's fight.
    { greivance >= 3:
        GUNSLINGER: Hell yeah, we'll rock it!
    - else:
        GUNSLINGER: If you say so.
    }
    -> wait_for_fight

=== wait_for_fight ===
# gate:fight
~ fightCount += 1
{ 
- fightCount == 1:
    -> talk_2
- fightCount == 2:
    -> talk_3_1
- else:
    -> talk_4
}



=== talk_2 ===
// after the fight, blood and dust settling
GUNSLINGER: You're getting sharper out there. A few more wins like that and the Wardens might actually remember our names.
NPC (Crowd Vendor): Hey, you two! Good show tonight. Drink's on the house for the new blood.

+ [Thank the Gunslinger for watching your back]
    John Apocalypse: Couldn't have pulled off that last dodge without you covering me.
    GUNSLINGER: Don't get used to it. But... yeah. We work well together.
    ~ greivance += 1
    -> dialog_hub

+ [Stay quiet and count your winnings]
    John Apocalypse: (Eyes on the coin pouch instead. Out here, trust is a currency I can't afford to spend carelessly.)
    GUNSLINGER: ...Not much of a talker after a win, huh.
    -> dialog_hub

=== talk_3_1 ===
GUNSLINGER: We're a good team. I'm gonna go grab a drink - don't get lonely without me.
John Apocalypse: (He melts into the crowd toward the taps. First time I've been alone all night.)

BROKER: You. Fighter. Buy you a drink?
John Apocalypse: Depends what it costs me.
BROKER: Smart. I like that already.
John Apocalypse: (Dressed too clean for the Pit. Warden money, probably.)
BROKER: I've been watching your matches. You're the one keeping that partnership alive - he just knows how to take a bow.

+ [He's doing his best.]John Apocalypse: He's doing his best.
    John Apocalypse: He's not dead weight. We wouldn't have won half these fights without him.
    ~ greivance += 1
    BROKER: Loyal. That's rare down here, and it won't buy you anything. But I might have something that will.
    -> ass_move

+ [Feels that way sometimes.]John Apocalypse: Feels that way sometimes.
    John Apocalypse: (Not going to badmouth him to a stranger. Doesn't mean I don't feel it, though.)
    John Apocalypse: Maybe. Doesn't change how the Pit splits the take.
    BROKER: No. But I might have something that could.
    -> ass_move

=== ass_move ===
+ [Hear him out]
    John Apocalypse: Alright. I'm listening.
    BROKER: The Wardens are bored. Same fights, same faces. They want a story the crowd won't stop talking about.
    John Apocalypse: What kind of story?
    BROKER: The kind where the loyal partner turns. Where the crowd watches someone they trusted, first.
    John Apocalypse: You're asking me to kill him.
    BROKER: I'm asking you to survive better than he does. Here.
    John Apocalypse: (A blade, small enough to hide in a sleeve. The edge is wrong - too dark, too wet.)
    BROKER: Laced. One scratch and it's done slow enough to look natural. Do it mid-fight, and nobody down here will ever know it wasn't the monsters.
    John Apocalypse: And if I don't?
    BROKER: Then you keep splitting your winnings and hoping he never finds a better deal than you. Everyone down here does, eventually.
    ~ knife = true
    ~ tellTruth = true
    -> talk_3_2

+ [Refuse the offer]
    John Apocalypse: Keep your knife. I'm not the Wardens' new attraction.
    ~ greivance += 1
    ~ tellTruth = true
    BROKER: Your loss. Offer doesn't come twice.
    John Apocalypse: Get out of my sight.
    BROKER: ...Your funeral, then. Figuratively. Probably.
    -> talk_3_2

=== talk_3_2 ===
GUNSLINGER: Man, water tastes best after a win. You know that feeling?
GUNSLINGER: Hey. You look rattled. Cold feet, or something else?

+ {tellTruth} [Tell him about the fishy stranger]
    John Apocalypse: Someone came up to me earlier. Wanted to turn me against you.
    GUNSLINGER: And? Did you say yes?
    John Apocalypse: Would I be telling you if I did?
    GUNSLINGER: ...Fair. Thanks for telling me. Not everyone would have.
    ~ greivance += 2
    -> dialog_hub

+ [Deny anything happened]
    John Apocalypse: You're starting to imagine things.
    GUNSLINGER: If you say so.
    -> dialog_hub

+ [Agree that things feel off]
    John Apocalypse: Yeah. Do you really think we can get out of here?
    GUNSLINGER: With me as your partner? A hundred percent.
    ~ greivance += 1
    -> dialog_hub

=== talk_4 ===
GUNSLINGER: That's our last break before the big one. Whoever's up there is finally putting their real money on us.
John Apocalypse: We've come a long way.

+ [Offer to watch his back, no matter what happens]
    John Apocalypse: Whatever's waiting up there - I've got you. That's not changing.
    GUNSLINGER: ...Didn't expect that from the one who was ready to gut me back at the start.
    ~ greivance += 2

+ [Stay silent and check your gear]
    John Apocalypse: (Some promises are easier kept unspoken - or not made at all.)

{ greivance >= 4:
    -> Ending2_Freedom
- else:
    -> Ending1_GUNSLINGER_Betrayal
}

/*
Ending1: Betrayal from Gunslinger (he becomes the final boss)
Ending2: Both walk free together
Ending3: You strike first and betray the Gunslinger with the knife (triggered from the in-arena kill action, not from dialogue)
Ending4: You refuse to fight back and die
*/

=== Ending1_GUNSLINGER_Betrayal ===
// Gunslinger levels a gun at John Apocalypse
John Apocalypse: What do you think you're doing?!
GUNSLINGER: Saving my own skin. I'm not sharing a payout with someone who's going to slow me down.
{ knife == true:
    GUNSLINGER: And don't act so shocked - I know about the knife you've been carrying. Guess we both had the same idea.
- else:
    GUNSLINGER: So. Guess you need to die instead.
}

+ [Fight back]
    John Apocalypse: You really think I'll let that happen?
    GUNSLINGER: No. I was hoping you'd give the crowd one last good show.
    // boss fight: Gunslinger
    -> Ending1_WinningButAtWhatCost

+ [Give up]
    John Apocalypse: You know... I really did like you. It was good, fighting at your side.
    -> Ending4_Death

=== Ending1_WinningButAtWhatCost ===
// the crowd roars
John Apocalypse: Did it really have to end like this?
John Apocalypse: (The Wardens toss a fresh contract before the blood's even dry. Another season. Another partner. Another cage.)
-> END

=== Ending2_Freedom ===
John Apocalypse: (The final bell rings and the gates - actually - open.)
GUNSLINGER: We did it. Both of us. Out.
John Apocalypse: (For the first time since I walked into the Pit, the word "freedom" doesn't feel like a lie the Wardens sell to keep people fighting.)
John Apocalypse: What now?
GUNSLINGER: Now? Now we figure out how to be people again.
-> END

=== Ending3_YourBetrayal ===
// reached from the in-arena "use the poisoned knife" action, not from dialog_hub
John Apocalypse: (I did it. The blade went in exactly where the Broker said it would.)
John Apocalypse: (So why is nobody pulling the monsters back? Why is the gate still open?)
BROKER: Oh - did you actually think we needed him gone to keep the show interesting? You just gave us a better one.
John Apocalypse: (No. No, no -)
John Apocalypse: (Everything he offered was a lie, and I bought it anyway. I'm going to die down here as a snitch, and nobody will even remember my name.)
-> END

=== Ending4_Death ===
John Apocalypse: Maybe... in another life, I get to be free.
-> END
