import type {ReactNode} from 'react';
import clsx from 'clsx';
import Heading from '@theme/Heading';
import styles from './styles.module.css';

type FeatureItem = {
  title: string;
  description: ReactNode;
};

const FeatureList: FeatureItem[] = [
  {
    title: 'One consistent pattern',
    description: (
      <>
        Every nested compose concept follows the same <code>Definition</code> + <code>Builder</code> shape,
        and every property with more than one YAML form gets the same overload set. Learn one, and the rest
        of the API reads the same way.
      </>
    ),
  },
  {
    title: 'Every form Compose accepts',
    description: (
      <>
        Short syntax, long syntax, and everything in between — the overload you call decides the shape written
        to YAML, so you're never fighting the library to get the form you want.
      </>
    ),
  },
  {
    title: 'Checked against real Compose',
    description: (
      <>
        Every golden-file fixture is validated in CI with the real <code>docker compose config</code>, not just
        compared to a saved string.
      </>
    ),
  },
];

function Feature({title, description}: FeatureItem) {
  return (
    <div className={clsx('col col--4')}>
      <div className="text--center padding-horiz--md">
        <Heading as="h3">{title}</Heading>
        <p>{description}</p>
      </div>
    </div>
  );
}

export default function HomepageFeatures(): ReactNode {
  return (
    <section className={styles.features}>
      <div className="container">
        <div className="row">
          {FeatureList.map((props, idx) => (
            <Feature key={idx} {...props} />
          ))}
        </div>
      </div>
    </section>
  );
}
