import matplotlib.pyplot as plt
import math
class Upgrade:
    def __init__(self, base, mult):
        self.base = base
        self.mult = mult
    
    def cost(self, numberOfUpgrades):
        return self.base*pow(self.mult, numberOfUpgrades)

    def owned(self, budget):
        i = 0
        while self.cost(i+1) < budget:
            i = i+1
        return i

def genPoints(upgrade, budgets):
    y = []

    for i in budgets:
        y.append(upgrade.owned(i))
    return y

x = range(0, 1000000, 10)

y1 = genPoints(Upgrade(50, 1.07), x)
y2 = genPoints(Upgrade(50, 1.15), x)



plt.plot(x, y1, label='1.07')
plt.plot(x, y2, label='1.15')
  
plt.xlabel('x - budget')
plt.ylabel('y - number of upgrades')
plt.legend()
plt.show()