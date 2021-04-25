import matplotlib.pyplot as plt

def genPoints(baseCost, mult, nofPoints):
    x = []
    y = []

    for i in range(nofPoints):
        x.append(i)
        y.append(baseCost*pow(mult, i))
    return (x, y)

x1, y1 = genPoints(50, 1.07, 100)
x2, y2 = genPoints(50, 1.15, 100)

plt.plot(x1, y1, label='1.07')
plt.plot(x2, y2, label='1.15')
  
plt.xlabel('x - nof upgrades')
plt.ylabel('y - cost')
plt.yscale('log')
plt.legend()
plt.show()